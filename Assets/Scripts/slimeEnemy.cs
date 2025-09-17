using UnityEngine;
using System.Collections;


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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPos = gamemanager.instance.player.transform;
        attackTimer = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);

        if (distance <= detRange)
        {
            if (distance > attackRange)
            {
                Vector3 dir = (playerPos.position - transform.position).normalized;
                dir.y = 0;
                rb.linearVelocity = dir * speed;
                transform.rotation = Quaternion.LookRotation(dir);
            }
            else if (attackTimer >= attackRate)
            {
                Attack();
            }
        }
       

        attackTimer += Time.deltaTime;
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

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(jumpDelay);

        rb.linearVelocity = new Vector3(0,0,0);
        rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (isAttacking && collision.gameObject.CompareTag("Floor"))
        {
            if (shockwave != null)
            {
                Instantiate(shockwave, transform.position, Quaternion.identity);
            }
           
            isAttacking = false;
        }
    }

    
    public override void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());

        if(HP <= 0)
        {
           Destroy(gameObject);
            agent.SetDestination(gamemanager.instance.player.transform.position);
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
