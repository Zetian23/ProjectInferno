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
    [SerializeField] float jumpRate;

    float jumpCooldownTimer;
    float jumpTimer;
    Vector3 startJumpPos;
    Vector3 currPos;
    Vector3 playerLocationAtJump;
    bool isJumping;

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
            currPos = transform.position;
            startJumpPos = jumpPos.position;
        }

        if (!isJumping && isAttacking)
        {
            meleeAttack();  // If the attack is happening then do a melee attack, I check this so the attack doesnt happen again until this is done.
        }
        else if (isJumping && isAttacking)
        {
            currPos = Vector3.Lerp(currPos, playerLocationAtJump, jumpTimer);
            transform.position = currPos;
            jumpTimer += Time.deltaTime * jumpRate;
            if (transform.position.y <= 3.7f)
            {
                jumpTimer = 0;
                transform.position = new Vector3(currPos.x, 3.7f, currPos.z);
                isJumping = false;
                agent.stoppingDistance = stoppingDistOrig;
            }
        }

        if (isLowerred)
        {
            swingWeapon();
            isJumping = false;
        }

        if (gamemanager.instance.GetPhase() >= 2 && !isJumping)
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
        isAttacking = false;

        if(jumpCooldownTimer >= jumpCooldown)
        {
            jumpTimer += Time.deltaTime * jumpRate;
            isJumping = true;
            agent.stoppingDistance = 0;
            transform.position = Vector3.Lerp(currPos, startJumpPos, jumpTimer);
            if (transform.position.y >= startJumpPos.y)
            {
                isAttacking = true;
                jumpTimer = 0;
                jumpCooldownTimer = 0;
                currPos = transform.position;
                transform.position = currPos;
                playerLocationAtJump = gamemanager.instance.playerScript.transform.position;
            }
        }
    }

    public override void Attack()
    {
        if (!isJumping)
        {
            base.Attack();
        }
    }
}
