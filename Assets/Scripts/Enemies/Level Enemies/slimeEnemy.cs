using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class slimeEnemy : Enemy
{
    //for movement
    [SerializeField] float speed;
    [SerializeField] float detRange;
    [SerializeField] float attackRange;

    //for the jump attack
    [SerializeField] float jumpForce;
    [SerializeField] float jumpDelay;

    private Rigidbody rb;
    private bool isAttacking = false;
    private Transform playerPos;
    public playerController expGained;

    //for roaming
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;
    float roamTimer;
    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        agent = GetComponent<NavMeshAgent>();
        playerPos = gamemanager.instance.player.transform;
        startingPos = transform.position;
        attackTimer = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        setAnimLoco();
        float distance = Vector3.Distance(transform.position, playerPos.position);

        if (distance <= detRange)
        {
            agent.stoppingDistance = attackRange;
            if (distance > attackRange)
            {
                agent.isStopped = false;
                agent.SetDestination(playerPos.position);
            }
            else if (attackTimer >= attackRate)
            {
                Attack();
            }
        }
        else
        {
            if (agent.remainingDistance < 0.01f)
            {
                roamTimer += Time.deltaTime;
            }
            if (playerInTrigger && !canSeePlayer())
            {
                checkRoam();
            }
            else if (!playerInTrigger)
            {
                checkRoam();
            }
        }
        attackTimer += Time.deltaTime;
    }
    void setAnimLoco()
    {

        float agentSpeedCur = agent.velocity.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTranSpeed));
    }
    public override void Attack()
    {
        Debug.Log("Slime ATTACK triggered!");
        attackTimer = 0;
        StartCoroutine(JumpAttack());
    }

    private IEnumerator JumpAttack()
    {
        isAttacking = true;

        
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(jumpDelay);

        if (shockwave != null)
        {
            Instantiate(shockwave, transform.position, Quaternion.identity);
        }

        isAttacking = false;
        agent.isStopped = false;
    }
    
    public override void takeDamage(int amount)
    {
        if (HP > 0)
        {

            HP -= amount;
            agent.SetDestination(gamemanager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(0, 0, -1);
            Destroy(gameObject);
            CallGainEXP();
        }
    }
    public void CallGainEXP()
    {
        if (expGained != null)
        {
            expGained.gainEXP(5);
            Debug.Log("EXP gained");
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
}
