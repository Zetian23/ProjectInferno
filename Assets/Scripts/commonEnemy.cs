
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
//code written by William
public class CommonEnemyScript : Enemy, IDamage
{
    [SerializeField] GameObject weapon;

    
    [SerializeField] bool isMelee;
    [SerializeField] bool isFlying;

    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;

    //for animation
    [SerializeField] Animator anim;
    [SerializeField] float animTranSpeed;

    //for flying enemies
    [SerializeField] float speed;
   
    [SerializeField] float hoverHeight;
    [SerializeField] float chaseRange;

    public playerController expGained;

    float roamTimer;
    //enum enemyType { skeleton, demon }
    //[SerializeField] enemyType type;

    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        gamemanager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        setAnimLoco();

        attackTimer += Time.deltaTime;

        if (isFlying)
        {
            flyingEnemy();
        }
        else
        {
            groundEnemy();
        }

    }

    void groundEnemy()
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

    void flyingEnemy()
    {
        float dist = Vector3.Distance(transform.position, gamemanager.instance.player.transform.position);

        if(dist < chaseRange && canSeePlayer())
        {
            Vector3 target = gamemanager.instance.player.transform.position + Vector3.up * hoverHeight;

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            faceTarget();

            if(dist <= agent.stoppingDistance+ 1f)
            {
                Attack();
            }
        }
        else
        {
            checkFlying();
        }
    }

    void checkFlying()
    {
        roamTimer += Time.deltaTime;
        if (roamTimer >= roamPauseTimer)
        {
            roamFlying();
        }

    }

    void roamFlying()
    {
        roamTimer = 0;
        Vector3 target = gamemanager.instance.player.transform.position + Vector3.up * hoverHeight;
        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;
        ranPos.y = startingPos.y + hoverHeight;

        transform.position = Vector3.MoveTowards(transform.position, ranPos, speed * Time.deltaTime);
    }
    void setAnimLoco()
    {
        float agentSpeedCur = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeedCur, Time.deltaTime * animTranSpeed));
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
    
    public override void Attack()
    {
        attackTimer = 0;

        anim.SetTrigger("Shoot");
        anim.SetTrigger("Attack");
        if (isMelee)
        {
            meleeAttack();
        }
        else
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
                Instantiate(weapon, attackPos.position, transform.rotation);
        }
    }

    public override void takeDamage(int amount)
    {
        Debug.Log("Ow");
        if (HP > 0)
        {
            HP -= amount;

            if(!isFlying)
            {
                agent.SetDestination(gamemanager.instance.player.transform.position);
            }
            agent.SetDestination(gamemanager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
            CallGainEXP();
        }
    }

    public void CallGainEXP()
    {
        if(expGained != null)
        {
            expGained.gainEXP(5);
            Debug.Log("EXP gained");
        }
        
    }
    
}
