using UnityEngine;
// Code Written By Nathaniel King <3

// Phase 1: (HP >= 500)
// Slam Attack          | Club swings then attacks the player in a radius around it.
// Phase 2: (HP >= 250)
// Projectiles          | When the boss attacks it will also do ranged attacks if the player is out of the melee range.
// Phase 3: (HP < 250)
// Jumping Slam Attack  | The boss will have a cool down that makes it jump in the air then doing a larger area of splash damage.

public class envyAI : sinEnemy
{
    [SerializeField] GameObject lasers;

    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        isAttacking = false;            // Initializing that an attack is not happening.

        gamemanager.instance.SetBossText("Envy");                       // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.envy;          // Setting the bossType to the Wrath Boss.
        gamemanager.instance.currBoss = 3;                              // Setting the boss in gameManger of the index for the Boss.
        updateBossUI();                                                 // Initializing the boss UI.
    }

    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(500, 250);   // Checks the phases between the health periods.

        if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
    }

    public override void Attack()
    {
        attackTimer = 0;
        Instantiate(lasers, new Vector3(attackPos.position.x, attackPos.position.y + 1, attackPos.position.z), transform.rotation);
    }
}
