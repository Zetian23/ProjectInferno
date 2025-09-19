using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class flyingEnemyAI : Enemy
{
    [SerializeField] float speed;
    [SerializeField] float hoverHeight;
    [SerializeField] float chaseRange;
    [SerializeField] float stoppingDist;
    [SerializeField] float roamRadius;
    [SerializeField] float roampauseTime;
 
    [SerializeField] GameObject bullet;

    private Vector3 startPos;
    private Vector3 roamTarget;
    private float roamTimer;
    public playerController expGained;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       startPos = transform.position;
       roamTarget = startPos;
        attackTimer = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        if(gamemanager.instance.player == null)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        Vector3 playerPos = gamemanager.instance.player.transform.position;
        float dist = Vector3.Distance(transform.position, playerPos);

        if(dist <= chaseRange && canSeePlayer())
        {
            ChasePlayer(playerPos, dist);
        }
        else
        {
            Roam();
        }
    }

    private void ChasePlayer(Vector3 playerPos, float dist)
    {
        Vector3 dir = (playerPos - transform.position).normalized;

        Vector3 targetPos = playerPos - dir * stoppingDist;
        targetPos.y = playerPos.y + hoverHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if(dist > stoppingDist)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }

        

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, faceTargetSpeed * Time.deltaTime);
        }

        if(dist <= stoppingDist && attackTimer >= attackRate)
        {
            Attack();
        }
    }

    private void Roam()
    {
        roamTimer += Time.deltaTime;

        if(roamTimer >= roampauseTime || Vector3.Distance(transform.position, roamTarget) < 1f)
        {
            roamTimer = 0;
            Vector3 randomPos = Random.insideUnitSphere * roamRadius;
            randomPos += startPos;
            randomPos.y = startPos.y + hoverHeight;
            roamTarget = randomPos;
        }

        transform.position = Vector3.MoveTowards(transform.position, roamTarget, speed * Time.deltaTime);
    }

    public override void Attack()
    {
        attackTimer = 0;


        if (bullet != null && attackPos != null)
                Instantiate(bullet,attackPos.position, attackPos.rotation);
           
        
    }

    public override void takeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;
            //agent.SetDestination(gamemanager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
        if(HP <= 0)
        {
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
}
