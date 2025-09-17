using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel <3
// Completed

public class sinEnemy : Enemy
{
    [SerializeField] protected float rotTime;                   
    [SerializeField] protected float xRotAngle;                 
    [SerializeField] GameObject weaponPos;                        
    [SerializeField] protected List<Renderer> skinObjects;              // This is for all the parts that will flash when damaged.

    protected Color emissionColorOrig;  // This is the original emission color of the skin.

    protected int BHPOrig;      // This is for the amount of health the boss starts out with.
    public bool isInvinsible;   // This is if a boss is invinsible to any attacks.
    public bool weakSpotHit;    // This checks if a weakness has been struck.
    public bool isLust;         // This is if the boss is the lust one.
    public bool isKilled;       // Has the boss been killed?
    protected float rotTimer;   // 
    protected bool isSpinning;  // Is the boss spinning?
    protected bool isInSpecial; // Is the boss in the special move?
    protected bool isLowerred;  // 
    protected bool isAttacking; // 

    public void InitVar() 
    {
        if (isKilled) Destroy(gameObject);
        gamemanager.instance.bossUI.SetActive(true);                            // Showing the boss UI.
        gamemanager.instance.SetPhase(1);                                       // Initializing phase to the first phase.
        startSpeed = agent.speed;                                               // Initializing how fast the boss was initially set to.
        isInvinsible = false;                                                   // Initializing that there is no invensibility.
        colorOrg = skinObjects[0].material.color;                               // Initializing the original color of the bosses material.
        emissionColorOrig = skinObjects[0].material.GetColor("_EmissionColor"); // Initializing the original emission color of the bosses skin.
        attackTimer = 0;                                                        // Initializing the attack timer to zero.
        BHPOrig = HP;                                                           // Initializing the BHOrig to the amout of health it starts with.
        stoppingDistOrig = agent.stoppingDistance;                              // Initializing the starting stopping distance.
    }

    public override void faceTarget()   // This is used to rotate towards the player.
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);                                         // Intialize a rotation towards the player.
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);   // Moves the enemy over time towards the player.
    }

    public override void takeDamage(int amount) // This is an override of take damage on the base Enemy script.
    {
        if (!isInvinsible) {    // If invensible then it shouldn't take damage.
            if (HP > 0)     // If the health is more than zero.
            {
                HP -= amount;                   // Then subtract the amount of health taken,
                StartCoroutine(flashDamage());  // flash the skin material to show damage has been taken,
                updateBossUI();                 // and update the UI for the boss.
            }
            if (HP <= 0)    // If the health has been depleted.
            { 
                Destroy(gameObject);                                    // Then destroy this object,
                gamemanager.instance.bossHealthUI[2].SetActive(false);  //
                gamemanager.instance.youWin();                          // and win the level.
            }
        }
    }

    public override IEnumerator flashDamage()   // This is override used to show the boss has taken through all the skin.
    {
        for (int i = 0; i < skinObjects.Count; i++) // Loop through all the skin materials.
            skinObjects[i].material.SetColor("_EmissionColor", Color.red);  // Change the material emission to red.
        yield return new WaitForSeconds(0.1f);  // Wait a second.
        for (int i = 0; i < skinObjects.Count; i++) // Loop through all the skin materials.
            skinObjects[i].material.SetColor("_EmissionColor", emissionColorOrig);  // Change the material emission to the original emission color.
    }

    protected void checkHealth(int phase1HealthMin, int phase2HealthMin)    // Checks health between phases.
    {
        if (HP < phase1HealthMin) gamemanager.instance.SetPhase(2);    // If the health has gone lower than the first phase than change to phase two.
        if (HP < phase2HealthMin) gamemanager.instance.SetPhase(3);    // If the health has gone lower than the second phase than change to phase three.
    }

    public void updateBossUI()  // Used to change the health on the UI.
    {
        if (gamemanager.instance.GetPhase() == 1)
        {
            gamemanager.instance.bossHPBar[0].fillAmount = (float)HP / BHPOrig;   // When updated the health bar will be at the same vaule as this bosses health.
            gamemanager.instance.bossHealthUI[0].SetActive(true);
            gamemanager.instance.bossHealthUI[1].SetActive(false);
            gamemanager.instance.bossHealthUI[2].SetActive(false);
        }
        if (gamemanager.instance.GetPhase() == 2)
        {
            gamemanager.instance.bossHPBar[1].fillAmount = (float)HP / BHPOrig;   // When updated the health bar will be at the same vaule as this bosses health.
            gamemanager.instance.bossHealthUI[0].SetActive(false);
            gamemanager.instance.bossHealthUI[1].SetActive(true);
        }
        if (gamemanager.instance.GetPhase() == 3)
        {
            gamemanager.instance.bossHPBar[2].fillAmount = (float)HP / BHPOrig;   // When updated the health bar will be at the same vaule as this bosses health.
            gamemanager.instance.bossHealthUI[1].SetActive(false);
            gamemanager.instance.bossHealthUI[2].SetActive(true);
        }
    }

    protected IEnumerator swingWeapon()    // Set motion that brings down the sword and raises it.
    {
        rotTimer += Time.deltaTime;   // Incerement the amount of time the swing has happen.

        if (rotTimer < rotTime && !isLowerred)  // If the swing hasn't hit the landingRotation and timer is less than the time it needs to swing.
            weaponPos.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(60, 0, 0), rotTimer * 2);   // Then use Slerp (which is like Lerp but deals with spherical motions overtime) to move the sword down.
        else if (rotTimer < rotTime && isLowerred)  // If the sword has been lowerred and the timer is less than the time it needs to raise.
            weaponPos.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(xRotAngle, 0, 0), Quaternion.Euler(0, 0, 0), rotTimer * 2);   // Then move the sword back up to the starting LOCAL rotation.
        else if (rotTimer >= rotTime)   // If the timer has exceeded the time given.
        {
            rotTimer = 0;             // Set the timer back to zero.
            if (isLowerred)             // Check if the the sword is lowerred.
            {
                if (!isSpinning && gamemanager.instance.currBoss == 1) isInSpecial = false;
                isLowerred = false;                                                         // If so then set the islowerred to false as it has raised,
                isAttacking = false;                                                        // and isAttacking to false so that the boss can attack again.
            }
            else isLowerred = true;     // Also if isLowerred is not set to true then set it to true.
        }
        yield return null;  // Incerement after one frame.
    }

    protected override void meleeAttack()  // Base attack for when the boss is close up attacking.
    {
        attackTimer = 0;// Reset the timer so that the attack will happen again after a period of time.

        RaycastHit hit;
        if (!isLowerred)    // If the sword hasn't been lowered.
            StartCoroutine(swingWeapon());   // Then swing the sword down until it hits the landingRotation.  
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer)
            && isLowerred && rotTimer == 0 && gamemanager.instance.currBoss != 2) // Draws a ling with the attackDistance to see if the player is within the distance, if the sword has been lowerd and the swingTimer is set to 0.
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>(); // Initializing the IDamage script.

            if (dmg != null)    // Checks if the thing collided took damage.
            {
                dmg.takeDamage(attackDamage);   // Make the player take damage.
            }
        }
        else if (isLowerred && rotTimer == 0 && gamemanager.instance.currBoss == 2)
        {
            Instantiate(shockwave, shockwavePos.position, Quaternion.identity);
        }
        if (isLowerred) StartCoroutine(swingWeapon());    // If the sword has been lowered then raise the sword back to startingLocalRotaion.
    }

    public override void Attack()   // Once attackRate is equal to the attackTimer this will be called if the player is in the line of sight of the boss.
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance + 2f, ~ignoreLayer) && !isAttacking)   // If the player is 2 over the attack distance.
            isAttacking = true; // Then the attack is ready to be done.
    }

    public override void saveData(ref gameData data)
    {
        if (isKilled)
        {
            data.bossDefeated[gamemanager.instance.currBoss] = true;
        }
    }

    public override void loadData(gameData data)
    {
        isKilled = true;
    }
}