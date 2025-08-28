using UnityEngine;
// Code Written By Nathaniel King <3
// Completed

// Phase 1: (HP >= 1000)
    // Arcana I         | One big enemy with more health and more damage but really slow.
// Phase 2: (HP >= 500)
    // Arcana II        | During this two will have separate health of 250 and are little more fast and less damage.
// Phase 3: (HP < 500)
    // Arcana III       | Last bit of slimes that are really fast and deal less damage.

public class lustAI : sinEnemy
{
    [SerializeField] GameObject lustChildArcana;    // This is used to put the next arcana of lust boss that will spawn.
    [SerializeField] int lustArcana;                // This is used to set which arcana the current object is.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitVar();  // Initializing all the data in the sinEnemy.

        if(lustArcana == 1) // If the object initiated is of the first arcana.
        {
            gamemanager.instance.SetBossText("Lust");               // Setting the boss nametag to "Lust".
            gamemanager.instance.boss = gamemanager.bossType.lust;  // Setting the bossType to the Lust Boss.
            updateBossUI();                                         // Initializing the boss UI.
            gamemanager.instance.updateGameGoal(1, 0, 0);           // Add one boss to the game goal.
            gamemanager.instance.lustIIIArcana = 4;                 // Set the amount of third arcana sub enemies that will be spawned.
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
    }

    public override void Attack()   // Once attackRate is equal to the attackTimer this will be called if the player is in the line of sight of the boss.
    {
        meleeAttack();  // Does an melee attack when close to the player.
    }

    public override void takeDamage(int amount) // Override the takeDamage from the Enemy script.
    {
        if (HP > 0)     // If health is not delpenished.
        {
            HP -= amount;                   // Then loose health amount,
            StartCoroutine(flashDamage());  // Do a flashing animation,
            updateBossUI();                 // And update boss health bar.
        }
        if (HP <= 0)    // If health is gone.
        {
            if(lustChildArcana == null) // Check if the next child is non existant.
            {
                Destroy(gameObject);    // Destroy only this Object with no Instantiate.
            }
            else    // If there is an object in the lustChildArcana.
            {
                Instantiate(lustChildArcana, transform.position, transform.rotation);   // Then Instantiate
                Instantiate(lustChildArcana, transform.position, transform.rotation);   // two child enemies of this.

                if (lustArcana == 2)    // Check if this is the third arcana.
                    gamemanager.instance.lustIIIArcana--;   // If so then subtract one from the four that are made.

                if(gamemanager.instance.lustIIIArcana == 0) // If there is no third arcana lust bosses,
                    gamemanager.instance.updateGameGoal(-1, 0, 0);  // then subtract one boss from the game goal.

                Destroy(gameObject);    // Destroy this object when done.
            }
        }
    }
}