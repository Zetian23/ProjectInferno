using System.Collections;
using UnityEngine;
// Code Written By Nathaniel King <3
// Completed

// Phase 1: (HP >= 175)
    // Melee Attack     | Sword slash onto player.
// Phase 2: (HP >= 100)
    // Invensibility    | After a cooldown the boss will flash and be invensible to attacks.
// Phase 3: (HP < 100)
    // Sprinting        | Once hit to this point the boss will keep a sprinting speed until killed.

public class wrathAI : sinEnemy
{
    [SerializeField] GameObject Sword;                  // Get the Object where the sword is.
    [SerializeField] Color invensiblityEmissionColor;   // This will be the color that is flashed during flashInvensibily().
    [SerializeField] float invensibleCooldownTime;      // This is the time that the invensibility will be started.
    [SerializeField] float invensibleFlashTime;         // This is how long the flashes will take.
    [SerializeField] int invensibleFlashes;             // This is how many flashes will occur before the invensibility will last.

    Quaternion startingLocalRotation;   // This is where the sword was at when initialized.
    Quaternion landingRotation;         // This is where the sword will swing down to.
    float invensibleCooldownTimer;      // Timer that tracks the cooldown of the invensibilty skill.
    float invensibleFlashTimer;         // Timer that tracks the flash length.
    float swingTimer;                   // Timer that tracks how long the sword has been swung.
    float swingTime;                    // This is how long the sword will be swung for.
    float sprintSpeed;                  // How fast will the boss go after they start phase three.
    int currFlashes;                    // How many flashes have happened.
    bool flashed;                       // Has the enemy flashed invensibility.
    bool isAttacking;                   // Is the enemy attacking.
    bool isLowerred;                    // Is the sword down.

    private void Start()
    {
        InitVar();  // Initializes all of the bases varibles.

        startingLocalRotation = Sword.transform.localRotation;  // Initializing the LOCAL rotation of the Sword.
        landingRotation = Quaternion.Euler(90, 0, 0);           // Initializing where the sword will rotate to.

        isAttacking = false;            // Initializing that an attack is not happening.
        swingTime = 0.5f;               // Initializing that the time the swing will happen is half a second.
        currFlashes = 0;                // Initializing that the flashes haven't had any.
        sprintSpeed = agent.speed * 3;  // Initializing the speed that the boss will have when in phase three.

        gamemanager.instance.SetBossText("Wrath");              // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.wrath; // Setting the bossType to the Wrath Boss.
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.
        invensibleCooldownTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(175, 100);   // Checks the phases between the health periods.

        if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.

        if (isAttacking) meleeAttack(); // If the attack is happening then do a melee attack, I check this so the attack doesnt happen again until this is done.

        if (invensibleCooldownTimer >= invensibleCooldownTime && phase >= 2)    // Checks if the invensibilty cooldown is ready and if it is phase two.
        {
            isInvensible = true;                    // Set invensiblity to true so the boss doesn't take damage.
            StartCoroutine(flashInvensiblity());    // Calls the flashInvensibilty while the invensibilty is active.
        }

        if(phase == 3) agent.speed = sprintSpeed;   // Sets the speed of the boss when in phase three.
    }

    void meleeAttack()  // Base attack for when the boss is close up attacking.
    {
        attackTimer = 0;// Reset the timer so that the attack will happen again after a period of time.

        RaycastHit hit;
        if (!isLowerred)    // If the sword hasn't been lowered.
            StartCoroutine(swingSword());   // Then swing the sword down until it hits the landingRotation.  
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer) && isLowerred && swingTimer == 0) // Draws a ling with the attackDistance to see if the player is within the distance, if the sword has been lowerd and the swingTimer is set to 0.
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>(); // Initializing the IDamage script.

            if (dmg != null)    // Checks if the thing collided took damage.
            {
                dmg.takeDamage(attackDamage);   // Make the player take damage.
            }
        }
        if(isLowerred) StartCoroutine(swingSword());    // If the sword has been lowered then raise the sword back to startingLocalRotaion.
    }

    public override void Attack()   // Once attackRate is equal to the attackTimer this will be called if the player is in the line of sight of the boss.
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance + 2f, ~ignoreLayer) && !isAttacking)   // If the player is 2 over the attack distance.
            isAttacking = true; // Then the attack is ready to be done.
    }

    IEnumerator flashInvensiblity() // This will flash a color and work in the Invensibility all into one.
    {
        float t;                                                                    // Initialize the time that the Lerp() is at.
        invensibleFlashTimer += Time.deltaTime;                                     // Increment the time the flash happens for.
        t = Mathf.PingPong(Time.time, invensibleFlashTime) / invensibleFlashTime;   // Using the pingpong math method to use it like a sin wave where it starts at the bottom zero than goes to a certain time.

        if (!flashed && invensibleFlashTimer < invensibleFlashTime)    // If the invensibilty flash hasn't increased already and if the timer is less than the time it should flash for.
            for (int i = 0; i < skinObjects.Count; i++) // Then loop through and set each skin material to the flashing emission color.
                skinObjects[i].material.SetColor("_EmissionColor", (Color.Lerp(emissionColorOrig, invensiblityEmissionColor, t)) * 50); // Which is done with a Lerp() to do over time.
        else if (flashed && invensibleFlashTimer < invensibleFlashTime)  // If the invensibilty flash has increased already and if the timer is less than the time it should flash for.
            for (int i = 0; i < skinObjects.Count; i++) // Then loop through and set each skin material to the original emission color.
                skinObjects[i].material.SetColor("_EmissionColor", (Color.Lerp(invensiblityEmissionColor, emissionColorOrig, t)));  // Which is done with a Lerp() to do over time.
        else if (!flashed && invensibleFlashTimer >= invensibleFlashTime)   // If the flash has't happened and the timer is over or equal to the time given.
        {
            invensibleFlashTimer = 0;   // Then reset the timer,
            flashed = true;             // and set flashed to true since it has now happened.
        }
        else if (flashed && invensibleFlashTimer >= invensibleFlashTime)    // If the flash has happened and the timer is over or equal to the time given.
        {
            invensibleFlashTimer = 0;   // Then reset the timer,
            currFlashes++;              // incerment the amount of flashes that has happened in this invesiblity skill,
            flashed = false;            // and set the flashed to false as it has now returned to the original color.
        }
        if(currFlashes == invensibleFlashes)    // If the amount of flashes that have happen equal the amount set.
        {
            invensibleCooldownTimer = 0;    // Then reset the cooldown timer,
            currFlashes = 0;                // reset the amout of flashes that have happened,
            isInvensible = false;           // and set the invensibilty to false as the boss is no longer invensible.
        }

        yield return null;  // Incerement after one frame.
    }

    IEnumerator swingSword()    // Set motion that brings down the sword and raises it.
    {
        swingTimer += Time.deltaTime;   // Incerement the amount of time the swing has happen.

        if (swingTimer < swingTime && !isLowerred)  // If the swing hasn't hit the landingRotation and timer is less than the time it needs to swing.
            Sword.transform.localRotation = Quaternion.Slerp(startingLocalRotation, landingRotation, swingTimer * 2);   // Then use Slerp (which is like Lerp but deals with spherical motions overtime) to move the sword down.
        else if (swingTimer < swingTime && isLowerred)  // If the sword has been lowerred and the timer is less than the time it needs to raise.
            Sword.transform.localRotation = Quaternion.Slerp(landingRotation, startingLocalRotation, swingTimer * 2);   // Then move the sword back up to the starting LOCAL rotation.
        else if (swingTimer >= swingTime)   // If the timer has exceeded the time given.
        {
            swingTimer = 0;             // Set the timer back to zero.
            if (isLowerred)             // Check if the the sword is lowerred.
            {
                isLowerred = false;     // If so then set the islowerred to false as it has raised,
                isAttacking = false;    // and isAttacking to false so that the boss can attack again.
            }
            else isLowerred = true;     // Also if isLowerred is not set to true then set it to true.
        }

        yield return null;  // Incerement after one frame.
    }
}
