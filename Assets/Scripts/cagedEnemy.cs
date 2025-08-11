using UnityEngine;
// Code written by Nathaniel
public class cagedEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrg = model.material.color;
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (playerInTrigger)
        {
            playerDirection = gamemanager.instance.player.transform.position - transform.position;

            agent.SetDestination(gamemanager.instance.player.transform.position);

            if (attackTimer >= attackRate)
            {
                Attack();
            }
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
        }
    }

    public override void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInTrigger = false;
    }

    //public override void Attack()
    //{
    //    attackTimer = 0;
    //    Instantiate(bullet, attackPos.position, transform.rotation);
    //}

    public override void takeDamage(int amount)
    {
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
}