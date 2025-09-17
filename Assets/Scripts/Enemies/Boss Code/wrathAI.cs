using System.Collections;
using UnityEngine;
// Code Written By Nathaniel King <3
// Completed

// Phase 1: (HP >= 400)
// Melee Attack     | Sword slash onto player.
// Phase 2: (HP >= 200)
// Invinsibility    | After a cooldown the boss will flash and be invensible to attacks.
// Phase 3: (HP < 200)
// Spinning         | Once hit to this point the boss will do a spin move when invincible.

public class wrathAI : sinEnemy
{
    [SerializeField] GameObject Sword;                  // Get the Object of the sword itself.
    [SerializeField] Color invinsiblityEmissionColor;   // This will be the color that is flashed during flashInvensibily().
    [SerializeField] float invinsibleCooldownTime;      // This is the time that the invensibility will be started.
    [SerializeField] float invinsibleFlashTime;         // This is how long the flashes will take.
    [SerializeField] int invinsibleFlashes;             // This is how many flashes will occur before the invensibility will last.

    float invinsibleCooldownTimer;      // Timer that tracks the cooldown of the invensibilty skill.
    float invinsibleFlashTimer;         // Timer that tracks the flash length.
    float sprintSpeed;                  // How fast will the boss go after they start phase three.
    int currFlashes;                    // How many flashes have happened.
    float currYRot;                     // This is for knowing the rotation of the Boss for the spinning move.

    private void Start()
    {
        InitVar();  // Initializes all of the bases varibles.

        //gamemanager.instance.updateGameGoal(1, 0, 0);   // Add one boss to the game goal.

        isAttacking = false;            // Initializing that an attack is not happening.
        currFlashes = 0;                // Initializing that the flashes haven't had any.
        sprintSpeed = agent.speed * 3;  // Initializing the speed that the boss will have when in phase three.

        gamemanager.instance.SetBossText("Wrath");              // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.wrath; // Setting the bossType to the Wrath Boss.
        gamemanager.instance.currBoss = 1;                      // Setting the boss in gameManger of the index for the Boss.
        updateBossUI();                                         // Initializing the boss UI.
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.
        invinsibleCooldownTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(400, 200);   // Checks the phases between the health periods.
        
        if (!isInSpecial)
        {
            if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
        }

        if (isAttacking && !isSpinning) meleeAttack(); // If the attack is happening then do a melee attack, I check this so the attack doesnt happen again until this is done.

        if (invinsibleCooldownTimer >= invinsibleCooldownTime && gamemanager.instance.GetPhase() >= 2)    // Checks if the invensibilty cooldown is ready and if it is phase two.
        {
            isInvinsible = true;                    // Set invensiblity to true so the boss doesn't take damage.
            StartCoroutine(flashInvinsiblity());    // Calls the flashInvensibilty while the invensibilty is active.
        }

        if (gamemanager.instance.GetPhase() == 3 && isInvinsible)
        {
            agent.speed = sprintSpeed;   // Sets the speed of the boss when in phase three.
            agent.stoppingDistance = 8;
            if (!isSpinning && !isLowerred)
            {
                StartCoroutine(swingWeapon());
            }
            else if(!isSpinning && isLowerred)
            {
                StartCoroutine(twistSword());
                Sword.GetComponent<BoxCollider>().enabled = false;
                currYRot = transform.rotation.y;
            }
            else if(isLowerred && isSpinning)
            {
                if (currYRot > 360) currYRot = 1;
                else currYRot += 3;
                transform.rotation = Quaternion.Euler(transform.rotation.x, currYRot, transform.rotation.z);
                agent.SetDestination(gamemanager.instance.player.transform.position);   // Sets the position the boss needs to go as the javelin's rigidbody.
                Sword.GetComponent<BoxCollider>().enabled = true;
            }
        }

        if(!isInvinsible && isInSpecial && isLowerred && gamemanager.instance.GetPhase() == 3)
        {
            agent.stoppingDistance = stoppingDistOrig;
            agent.speed = startSpeed;
            if (isLowerred && isSpinning)
            {
                StartCoroutine(twistSword());
            }
            else if(isLowerred && !isSpinning)
            {
                StartCoroutine(swingWeapon());
            }
        }
    }

    IEnumerator flashInvinsiblity() // This will flash a color and work in the Invensibility all into one.
    {
        float t;                                                                    // Initialize the time that the Lerp() is at.
        invinsibleFlashTimer += Time.deltaTime;
        t = Mathf.PingPong(Time.time, invinsibleFlashTime) / invinsibleFlashTime;   // Using the pingpong math method to use it like a sin wave where it starts at the bottom zero than goes to a certain time.

        if (invinsibleFlashTimer < invinsibleFlashTime)    // If the invensibilty flash hasn't increased already and if the timer is less than the time it should flash for.
        {
            for (int i = 0; i < skinObjects.Count; i++) // Then loop through and set each skin material to the flashing emission color.
                skinObjects[i].material.SetColor("_EmissionColor", (Color.Lerp(emissionColorOrig, invinsiblityEmissionColor, t)) * 50f); // Which is done with a Lerp() to do over time.
            yield return null;                                                                                  // Continues after a frame.
        }
        else if (invinsibleFlashTimer >= invinsibleFlashTime)   // If the flash has't happened and the timer is over or equal to the time given.
        {
            invinsibleFlashTimer = 0;   // Then reset the timer,
            currFlashes++;             // and set flashed to true since it has now happened.
        }
        if(currFlashes == invinsibleFlashes)    // If the amount of flashes that have happen equal the amount set.
        {
            for (int i = 0; i < skinObjects.Count; i++) // Then loop through and set each skin material to the flashing emission color.
                skinObjects[i].material.SetColor("_EmissionColor", emissionColorOrig); // Which is done with a Lerp() to do over time.
            invinsibleCooldownTimer = 0;    // Then reset the cooldown timer,
            currFlashes = 0;                // reset the amout of flashes that have happened,
            isInvinsible = false;           // and set the invensibilty to false as the boss is no longer invensible.
        }
    }

    IEnumerator twistSword()
    {
        rotTimer += Time.deltaTime;   // Incerement the amount of time the swing has happen.
        isInSpecial = true;

        if (rotTimer < rotTime && !isSpinning)  // If the swing hasn't hit the landingRotation and timer is less than the time it needs to swing.
            Sword.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(-45, 0, 0), Quaternion.Euler(-45, 0, 105), rotTimer * 2);   // Then use Slerp (which is like Lerp but deals with spherical motions overtime) to move the sword down.
        else if (rotTimer < rotTime && isSpinning)  // If the sword has been lowerred and the timer is less than the time it needs to raise.
            Sword.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(-45, 0, 105), Quaternion.Euler(-45, 0, 0), rotTimer * 2);   // Then move the sword back up to the starting LOCAL rotation.
        else if (rotTimer >= rotTime)   // If the timer has exceeded the time given.
        {
            rotTimer = 0;             // Set the timer back to zero.
            if (isSpinning)             // Check if the the sword is lowerred.
            {
                isSpinning = false;     // If so then set the islowerred to false as it has raised,
                isAttacking = false;    // and isAttacking to false so that the boss can attack again.
            }
            else isSpinning = true;     // Also if isLowerred is not set to true then set it to true.
        }

        yield return null;  // Incerement after one frame.
    }
}
