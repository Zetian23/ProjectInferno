using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Code Written By Nathaniel King <3

// Phase 1: (HP >= 500)
// Reactivate           | Turrets in the boss areana will reactivate after a time if been downed. Also boss has ranged attacks.
// Phase 2: (HP >= 250)
// Long Lasers          | When the boss attacks it will also do ranged attacks if the player is out of the melee range.
// Phase 3: (HP < 250)
// Shockwave Laser      | When the boss shoots lasers they will blow up where it lands.

public class envyAI : sinEnemy
{
    [SerializeField] GameObject phase3Laser;
    [SerializeField] GameObject phase2Laser;
    [SerializeField] float downTurretsTime;
    [SerializeField] float spinCooldownTime;
    [SerializeField] float spinTime;

    List<turretEnemy> turretList;
    float downTurretsTimer;
    float spinCooldownTimer;
    float spinTimer;
    float currYRot;

    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        isAttacking = false;            // Initializing that an attack is not happening.
        isSpinning = false;

        IEnumerable<turretEnemy> list = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<turretEnemy>();
        turretList = new List<turretEnemy>(list);

        gamemanager.instance.SetBossText("Envy");                       // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.envy;          // Setting the bossType to the Wrath Boss.
        gamemanager.instance.currBoss = 3;                              // Setting the boss in gameManger of the index for the Boss.
        updateBossUI();                                                 // Initializing the boss UI.
    }

    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.
        spinCooldownTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(500, 250);   // Checks the phases between the health periods.
        if (!isSpinning)
        {
            if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
        }

        if(gamemanager.instance.GetPhase() >= 2 && !isSpinning && spinCooldownTimer >= spinCooldownTime)
        {
            currYRot = transform.rotation.y;
            isSpinning = true;
        }
        else if(gamemanager.instance.GetPhase() >= 2 && isSpinning && spinCooldownTimer >= spinCooldownTime)
        {
            StartCoroutine(spinAttack());
        }

        if (gamemanager.instance.GetPhase() == 3) projectile = phase3Laser;

        if (isAttacking)
        {
            rangedAttack();
        }

        CheckTurrets();
    }

    private void CheckTurrets()
    {
        downTurretsTimer += Time.deltaTime;
        if (downTurretsTimer >= downTurretsTime)
        {
            for (int i = 0; i < turretList.Count; i++)
            {
                if (turretList[i].isDown)
                {
                    turretList[i].HP = turretList[i].HPOrig;
                    turretList[i].isDown = false;
                }
            }
            downTurretsTimer = 0;
        }
    }

    IEnumerator spinAttack()
    {
        spinTimer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.x, currYRot, transform.rotation.z);
        if (currYRot > 360) currYRot = 1;
        else currYRot += 3;
        if(spinTimer >= .25f && phase2Laser.activeInHierarchy == false)
        {
            phase2Laser.SetActive(true);
        }
        if(spinTimer >= spinTime)
        {
            isSpinning = false;
            spinTimer = 0;
            spinCooldownTimer = 0;
            phase2Laser.SetActive(false);
        }
        yield return null;
    }
}
