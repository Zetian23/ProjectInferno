using UnityEngine;
using UnityEngine.AI;
using System.Collections;
// Code written by Nathaniel King <3 and William
// Base class for any enemies that will be created throughout Project Inferno
public class Enemy : MonoBehaviour, IDamage, IFreezable, ISavedData
{
    // These SerializedField will show up in any enemy that inherits from this parent
    [SerializeField] protected LayerMask ignoreLayer;

    [SerializeField] public Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform headPos;

    [SerializeField] public int HP;

    [SerializeField] public int faceTargetSpeed;

    [SerializeField] public float attackRate;
    [SerializeField] public float attackDistance;
    [SerializeField] public int attackDamage;
    [SerializeField] public int FOV;
    [SerializeField] public Transform attackPos;

    [SerializeField] public Animator anim;
    [SerializeField] public float animTranSpeed;
    [SerializeField] protected GameObject shockwave;
    [SerializeField] protected float shockwaveRadius;
    [SerializeField] protected int shockwaveDamage;
    [SerializeField] protected Transform shockwavePos;
    protected Color colorOrg;

    protected Vector3 playerDirection;

    protected float attackTimer;
    protected float angleToPlayer;
    protected float stoppingDistOrig;
    protected float startSpeed;
    protected float ogAnimSpeed;
    public int HPOrig;
    protected int ogAttackDam;
   
    //for freeze
    public bool isFroze = false;
    private float ogSpeed = 0;

    protected bool playerInTrigger;

    public bool canSeePlayer()
    {
        playerDirection = gamemanager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(headPos.position, playerDirection);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                if(gamemanager.instance.currBoss != 5)
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }

    protected virtual void meleeAttack()
    {
        attackTimer = 0;

          RaycastHit hit;
          if (Physics.Raycast(headPos.position, playerDirection, out hit, attackDistance, ~ignoreLayer))
          {
                IDamage dmg = hit.collider.GetComponent<IDamage>();

                if (dmg != null)
                {
                    dmg.takeDamage(attackDamage);
                }
          }
    }

    virtual public void faceTarget() { }
    public virtual void Attack() { }
    public virtual void takeDamage(int amount) { }

    public virtual IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = colorOrg;
    }

    public void slothSlow(float percent)
    {
       startSpeed *= percent;
    }

    public void freeze()
    {
        isFroze = true;

        if (agent != null)
        {
            ogSpeed = agent.speed;
            agent.speed = 0;
            
            agent.isStopped = true;
            
        }

        if (anim != null)
        {
            ogAnimSpeed = anim.speed;
            anim.speed = 0;
        }
        ogAttackDam = attackDamage;
        attackDamage = 0;
    }

    public void unfreeze()
    {
       
        isFroze = false;

        if (agent != null)
        {
           
            agent.speed = ogSpeed;
           
            agent.isStopped = false;
        }

        if (anim != null)
        {
            anim.speed = ogAnimSpeed;
        }
        attackDamage = ogAttackDam;
    }

    public virtual void loadData(gameData data)
    {
        throw new System.NotImplementedException();
    }

    public virtual void saveData(ref gameData data)
    {
        throw new System.NotImplementedException();
    }
}