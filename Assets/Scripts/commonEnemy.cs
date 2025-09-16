
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
//code written by William
public class CommonEnemyScript : Enemy
{
    [SerializeField] GameObject weapon;

    
    [SerializeField] bool isMelee;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTimer;
   
    public playerController expGained;

    
    float roamTimer;

    //for dodging
    [SerializeField] float dodgeDist;
    [SerializeField] float dodgeSpeed;
    [SerializeField] float dodgeCooldown;
    [SerializeField] float dodgeTime;

    private bool isDodging = false;
    private float dodgeTimer = 0;

    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        gamemanager.instance.updateGameGoal(0, 0, 1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        setAnimLoco();

        attackTimer += Time.deltaTime;

        if (dodgeTimer > 0)
        {
            dodgeTimer -= Time.deltaTime;
        }

        if (!isDodging)
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
        if(canSeePlayer() && Random.value < 0.1f)
        {
            TryDodge();
        }
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

    public override void faceTarget()
    {
        if (!isFroze)
        {
            Quaternion rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
        }
    }
    
    public override void Attack()
    {
        attackTimer = 0;

        
       
        if(isMelee)
        {
            if (!isFroze)
            {
                anim.SetTrigger("Attack");
                meleeAttack();
            }
        }
        else
        {
            if (!isFroze)
            {
                anim.SetTrigger("Shoot");
                if (agent.remainingDistance <= agent.stoppingDistance)
                    Instantiate(weapon, attackPos.position, transform.rotation);
            }
        }
    }

    public override void takeDamage(int amount)
    {
        Debug.Log("Ow");
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
        if(expGained != null)
        {
            expGained.gainEXP(5);
            Debug.Log("EXP gained");
        }
        
    }

    private void TryDodge()
    {
        if(!isDodging && dodgeTimer <= 0)
        {
            StartCoroutine(Dodge());
        }
    }
    
    private IEnumerator Dodge()
    {
            isDodging = true;
            dodgeTimer = dodgeCooldown;

            Vector3 playerDir = (gamemanager.instance.player.transform.position - transform.position).normalized;
            Vector3 dodgeDir = Vector3.Cross(playerDir, Vector3.up).normalized;

            if (Random.value > 0.5)
            {
                dodgeDir = -dodgeDir;
            }
            float elapsed = 0;
            while (elapsed < dodgeTime)
            {
                agent.Move(dodgeDir * dodgeSpeed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
        
        isDodging = false; 
    }
}
