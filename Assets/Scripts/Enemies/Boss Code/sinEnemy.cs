using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel <3
// Completed

public class sinEnemy : Enemy
{
    [SerializeField] protected List<Renderer> skinObjects;  // This is for all the parts that will flash when damaged.

    protected Color emissionColorOrig;  // This is the original emission color of the skin.

    protected int BHPOrig;      // This is for the amount of health the boss starts out with.
    public bool isInvinsible;   // This is if a boss is invinsible to any attacks.
    public bool weakSpotHit;    // This checks if a weakness has been struck.
    public bool isLust;         // This is if the boss is the lust one.

    public void InitVar() 
    {
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
                gamemanager.instance.bossHealthUI[2].SetActive(false);
                gamemanager.instance.youWin();                          // and win the level.
            }
        }
    }

    protected virtual void phaseChange() { }

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
}