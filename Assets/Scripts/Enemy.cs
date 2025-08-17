using UnityEngine;
using UnityEngine.AI;
using System.Collections;
// Code written by Nathaniel and William
// Base class for any enemies that will be created throughout Project Inferno
public class Enemy : MonoBehaviour, IDamage
{
    // These SerializedField will show up in any enemy that inherits from this parent

    [SerializeField] public Renderer model;        // The enemies renderer made for that enemy or enemy prefab
    [SerializeField] public NavMeshAgent agent;    // The agent that seperate enemies will have to have pathing

    [SerializeField] public int HP;

    [SerializeField] public int faceTargetSpeed;

    [SerializeField] public float attackRate;
    [SerializeField] public float attackDistance;
    [SerializeField] public int attackDamage;
    [SerializeField] public int FOV;
    [SerializeField] public Transform attackPos;


    protected Color colorOrg;

    protected Vector3 playerDirection;         // In the child classes this will be used to update in that class based on the player direction.

    protected float attackTimer;               // Each enemy will have different time it takes to attack.
    protected float angleToPlayer;
    protected float stoppingDistOrig;

    protected bool playerInTrigger;            // Player enters the area where the enemy will be aware of the player.

    public bool canSeePlayer()
    {
        playerDirection = gamemanager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(transform.position, playerDirection);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit))
        {
            // Hey I can see you!!!
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                agent.SetDestination(gamemanager.instance.player.transform.position);

                if (attackTimer >= attackRate)
                {
                    Attack();
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    virtual public void faceTarget() { }    // Basic method that keeps the enemy faced to the player after the enemy is at the desired position,
                                            // this will have logic in the update of the child enemy script.
    public virtual void Attack() { }        // Method that is called when an enemy attack, which will be different in the child classes.
    public virtual void takeDamage(int amount) { }    // Method that is called when the enemy takes damage based on the Idamage delt from the player.

    protected IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrg;
    }

}