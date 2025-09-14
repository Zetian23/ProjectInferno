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
    [SerializeField] GameObject shockwave;
    [SerializeField] float shockwaveRadius;
    [SerializeField] int shockwaveDamage;

    private Rigidbody Rb;
    private bool isAttacking = false;
    private Transform playerPos; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
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
                Rb.linearVelocity = dir * speed;
                transform.rotation = Quaternion.LookRotation(dir);
            }
            else if (attackTimer >= attackRate)
            {
                Attack();
            }
        }
        else
        {
            Rb.linearVelocity = new Vector3(0, Rb.angularVelocity.y, 0);
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

        Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(jumpDelay);

        Rb.linearVelocity = new Vector3(0,0,0);
        Rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
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
        }
    }

}
