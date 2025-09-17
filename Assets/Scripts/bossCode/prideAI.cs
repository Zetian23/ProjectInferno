using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class prideAI : sinEnemy
{
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject bullet;
    playerController playerScript;

    List<bool> playerSkills;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        playerScript = gamemanager.instance.playerScript;
        playerSkills = playerScript.getPlayersUpgrade();

        attackDamage = playerScript.getWeaponList()[playerScript.getWeaponIndex()].shootDamage;
        attackDistance = playerScript.getWeaponList()[playerScript.getWeaponIndex()].shootDist;
        //attackRate = playerScript.getWeaponList()[playerScript.getWeaponIndex()].shootRate;
        //shootEffect = gamemanager.instance.playerScript.getWeaponList()[gamemanager.instance.playerScript.getWeaponIndex()].shootEffect;

        gunModel.GetComponent<MeshFilter>().sharedMesh = playerScript.getWeaponList()[playerScript.getWeaponIndex()].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = playerScript.getWeaponList()[playerScript.getWeaponIndex()].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

        isAttacking = false;            // Initializing that an attack is not happening.

        gamemanager.instance.SetBossText("Pride");                      // Setting the boss nametag to "Wrath".
        gamemanager.instance.boss = gamemanager.bossType.pride;         // Setting the bossType to the Wrath Boss.
        gamemanager.instance.currBoss = 6;                              // Setting the boss in gameManger of the index for the Boss.
        updateBossUI();                                                 // Initializing the boss UI.
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        checkHealth(1000, 500);   // Checks the phases between the health periods.

        if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
    }

    public override void Attack()
    {
        attackTimer = 0;
        //playerScript.getWeaponList()[playerScript.getWeaponIndex()].ammoCur--;

        Instantiate(bullet, gunModel.transform.position, gunModel.transform.rotation);
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, attackDistance, ~ignoreLayer))
        //{
        //    //Debug.Log(hit.collider.name);

        //    //Instantiate(playerScript.shootEffect, hit.point, Quaternion.identity);

        //    IDamage dmg = hit.collider.GetComponent<IDamage>();

        //    if (dmg != null)
        //    {
        //        //Wrath
        //        if (playerSkills[1])
        //        {
        //            dmg.takeDamage((int)(attackDamage * playerScript.wrathDamageMult * (playerScript.DamageLevelUp * playerScript.level + 1)));
        //        }
        //        else
        //        {
        //            dmg.takeDamage((int)(attackDamage * (playerScript.DamageLevelUp * playerScript.level + 1)));
        //        }

        //        //Sloth
        //        if (playerSkills[0])
        //        {
        //            dmg.slothSlow(playerScript.slothSpeedReduction);
        //        }

        //        if (playerSkills[3])
        //        {
        //            takeDamage((int)(attackDamage * playerScript.envyHealPercent));
        //        }
        //    }
        //}
    }
}
