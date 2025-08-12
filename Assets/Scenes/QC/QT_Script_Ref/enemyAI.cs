using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;


    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    

    Color colorOrig;

    float shootTimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDistOrig;
    
    bool playerInTrigger;

    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;   
        gamemanager.instance.updateGameGoal(1);
        startingPos = transform.position; // Store the starting position of the enemy
        stoppingDistOrig = agent.stoppingDistance; // Store the original stopping distance of the agent
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (agent.remainingDistance < 0.01f)
            roamTimer = Time.deltaTime;

        // Check if the player is within the trigger collider
        if (playerInTrigger && !canSeePlayer())
        {
            checkRoam();
        }
        else if(!playerInTrigger)
        {
            checkRoam();


        }

    }

    void checkRoam()
    {
        if (roamTimer >= roamPauseTime && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0; // Set the stopping distance to 0 to allow roaming

        Vector3 ranPos = Random.insideUnitSphere * roamDist; // Generate a random position within the roaming distance
        ranPos += startingPos; // Add the starting position to the random position
        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1); // Sample the nearest valid position on the NavMesh
        agent.SetDestination(hit.position); // Set the agent's destination to the sampled position
    }

    // Check if the enemy can see the player FINISH LOGIC
    bool canSeePlayer()
    {

        playerDir = gamemanager.instance.player.transform.position - transform.position; // Calculate the direction to the player
        angleToPlayer = Vector3.Angle(playerDir, transform.forward); // Calculate the angle between the enemy's forward direction and the direction to the player
        Debug.DrawRay(transform.position, playerDir); // Draw a ray from the enemy to the player for debugging

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            // Hey I can see the player!!
            if (hit.collider.CompareTag("Player")&& angleToPlayer <= FOV)
            {

                agent.SetDestination(gamemanager.instance.player.transform.position); // Set the agent's destination to the player's position

                if (shootTimer >= shootRate)
                {
                    shoot();
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }
                agent.stoppingDistance = stoppingDistOrig; // Reset the stopping distance to the original value
                return true; // If the angle is within the field of view, return true
            }
        }
        agent.stoppingDistance = 0; // Set the stopping distance to 0 to allow roaming
        return false; // If the angle is outside the field of view, return false
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // This function is called when the player enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    // This function is called when the player exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0; // Set the stopping distance to 0 to allow roaming
        }
    }
    
    void shoot() // This function is called when the enemy shoots a bullet
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation); // Instantiate a bullet at the shoot position with the enemy's rotation
    }

    public void takeDamage(int amount)
    {
        // Implement damage logic here
        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(flashRed());
        }
        

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }
}
