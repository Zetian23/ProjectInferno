
using UnityEngine;
using UnityEngine.AI;
//code written by William
public class CommonEnemyScript : Enemy, IDamage
{
    [SerializeField] GameObject weapon;

    
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;
    


    float roamTimer;
    //enum enemyType {ranged, melee, flying, idle}
    //[SerializeField] enemyType type;

    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        gamemanager.instance.updateGameGoal(0, 1, 0);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (agent.remainingDistance < 0.01f)
        {
            roamTimer += Time.deltaTime;
        }

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
        if (roamTimer >= roamPauseTimer && agent.remainingDistance < 0.01f)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;

        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }


    //public void checkEnemyType()
    //{
    //    if (type == enemyType.ranged || type == enemyType.idle)
    //    {

    //    }
    //}

    public override void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }
    public override void Attack()
    {
        attackTimer = 0;
        Instantiate(weapon, attackPos.position, transform.rotation);
    }

    public override void takeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;
            agent.SetDestination(gamemanager.instance.player.transform.position);
            StartCoroutine(flashRed());
        }
        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(0, -1, 0);
            Destroy(gameObject);
        }
    }

}
