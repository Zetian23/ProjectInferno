using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel <3
// Completed

public class sinEnemy : Enemy
{
    [SerializeField] protected LayerMask ignoreLayer;   // This is set for anything that needs to be ignored in the attacks.

    [SerializeField] protected List<Renderer> skinObjects;  // This is for all the parts that will flash when damaged.

    protected Color emissionColorOrig;  // This is the original emission color of the skin.

    protected int phase;        // This is the current phase that the boss is on when needed.
    protected int BHPOrig;      // This is for the amount of health the boss starts out with.
    public bool isInvensible;   // This is if a boss is invinsible to any attacks.
    public bool weakSpotHit;    // This checks if a weakness has been struck.

    public void InitVar()
    {
        gamemanager.instance.updateGameGoal(1, 0, 0);                           // Updating boss total to the game goal.
        isInvensible = false;                                                   // Initializing that there is no invensibility.
        colorOrg = model.material.color;                                        // Initializing the original color of the bosses material.
        emissionColorOrig = skinObjects[0].material.GetColor("_EmissionColor"); // Initializing the original emission color of the bosses skin.
        phase = 1;                                                              // Initializing phase to the first phase.
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
        if (!isInvensible) {    // If invensible then it shouldn't take damage.
            if (HP > 0)     // If the health is more than zero.
            {
                HP -= amount;                   // Then subtract the amount of health taken,
                StartCoroutine(flashDamage());  // flash the skin material to show damage has been taken,
                updateBossUI();                 // and update the UI for the boss.
            }
            if (HP <= 0)    // If the health has been depleted.
            { 
                gamemanager.instance.updateGameGoal(-1, 0, 0);  // Then update the count of bosses on the game goal
                Destroy(gameObject);                            // and destroy this object.
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

    protected virtual void meleeAttack()  // Base attack for when the boss is close up attacking.
    {
        attackTimer = 0;// Reset the timer so that the attack will happen again after a period of time.

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer)) // Draws a ling with the attackDistance to see if the player is within the distance.
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>(); // Initializing the IDamage script.

            if (dmg != null)    // Checks if the thing collided took damage.
            {
                dmg.takeDamage(attackDamage);   // Make the player take damage.
            }
        }
    }

    protected void checkHealth(int phase1HealthMin, int phase2HealthMin)    // Checks health between phases.
    {
        if (HP < phase1HealthMin) phase = 2;    // If the health has gone lower than the first phase than change to phase two.
        if (HP < phase2HealthMin) phase = 3;    // If the health has gone lower than the second phase than change to phase three.
    }

    public void updateBossUI()  // Used to change the health on the UI.
    {
        gamemanager.instance.bossHPBar.fillAmount = (float) HP / BHPOrig;   // When updated the health bar will be at the same vaule as this bosses health.
    }
}