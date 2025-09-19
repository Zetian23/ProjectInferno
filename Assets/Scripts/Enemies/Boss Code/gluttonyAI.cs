using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
// Code Written By Nathaniel King <3

// Phase 1: (HP >= 500)
// Slam Attack          | Club swings then attacks the player in a radius around it.
// Phase 2: (HP >= 250)
// Projectiles          | When the boss attacks it will also do ranged attacks if the player is out of the melee range.
// Phase 3: (HP < 250)
// Jumping Slam Attack  | The boss will have a cool down that makes it jump in the air then doing a larger area of splash damage.

public class gluttonyAI : sinEnemy
{
    [SerializeField] Transform jumpPos;
    [SerializeField] float jumpCooldown;

    float jumpCooldownTimer;
    Vector3 playerLocationAtJump;
    bool isJumping;
    bool isLanded;

    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        isAttacking = false;            // Initializing that an attack is not happening.

        gamemanager.instance.SetBossText("Gluttony");                   // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.gluttony;      // Setting the bossType to the Wrath Boss.
        gamemanager.instance.currBoss = 2;                              // Setting the boss in gameManger of the index for the Boss.
        updateBossUI();                                                 // Initializing the boss UI.
    }

    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(500, 250);   // Checks the phases between the health periods.

        if (!isJumping)
        {
            if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
        }

        if (isAttacking && gamemanager.instance.GetPhase() != 3)
        {
            meleeAttack();  // If the attack is happening then do a melee attack, I check this so the attack doesnt happen again until this is done.
        }
        else if (isJumping && isAttacking && gamemanager.instance.GetPhase() == 3)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            if (isLanded)
            {
                meleeAttack();
                isJumping = false;
                isLanded = false;
            }
        }

        if (isLowerred)
        {
            swingWeapon();
        }

        if (gamemanager.instance.GetPhase() == 2)
        {
            rangedAttack();
        }

        if(gamemanager.instance.GetPhase() == 3 && !isAttacking)
        {
            jumpAttack();
        }
    }

    protected override void rangedAttack()
    {
        RaycastHit hit;
        if (!Physics.Raycast(headPos.position, playerDirection, out hit, attackDistance, ~ignoreLayer) && playerInTrigger && attackTimer >= attackRate) // Draws a ling with the attackDistance to see if the player is within the distance.
        {
            base.rangedAttack();
            attackTimer = 0;
        }
    }

    void jumpAttack()
    {
        jumpCooldownTimer += Time.deltaTime;

        if(jumpCooldownTimer >= jumpCooldown)
        {
            isJumping = true;
            agent.stoppingDistance = 0;
            if (playerLocationAtJump == null) playerLocationAtJump = gamemanager.instance.player.transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            if (transform.position == jumpPos.position)
            {
                isAttacking = true;
                jumpCooldownTimer = 0;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("Platform")) isLanded = true;
    }
}
