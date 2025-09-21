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
    [SerializeField] GameObject Sword;
    [SerializeField] Color invinsiblityEmissionColor;
    [SerializeField] float invinsibleCooldownTime;
    [SerializeField] float invinsibleFlashTime;
    [SerializeField] int invinsibleFlashes;

    float invinsibleCooldownTimer;
    float invinsibleFlashTimer;
    float sprintSpeed;
    int currFlashes;
    float currYRot;

    private void Start()
    {
        InitVar();

        isAttacking = false;
        currFlashes = 0;
        sprintSpeed = agent.speed * 3;

        gamemanager.instance.SetBossText("Wrath");
        gamemanager.instance.boss = gamemanager.bossType.wrath;
        gamemanager.instance.currBoss = 1;
        updateBossUI();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        invinsibleCooldownTimer += Time.deltaTime;

        checkHealth(400, 200);
        
        if (!isInSpecial)
        {
            if (playerInTrigger && canSeePlayer()) { }
        }

        if (isAttacking && !isSpinning) meleeAttack();

        if (invinsibleCooldownTimer >= invinsibleCooldownTime && gamemanager.instance.GetPhase() >= 2)
        {
            isInvinsible = true;
            StartCoroutine(flashInvinsiblity());
        }

        if (gamemanager.instance.GetPhase() == 3 && isInvinsible)
        {
            agent.speed = sprintSpeed;
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
                agent.SetDestination(gamemanager.instance.player.transform.position);
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

    IEnumerator flashInvinsiblity()
    {
        float t;
        invinsibleFlashTimer += Time.deltaTime;
        t = Mathf.PingPong(Time.time, invinsibleFlashTime) / invinsibleFlashTime;

        if (invinsibleFlashTimer < invinsibleFlashTime)
        {
            for (int i = 0; i < skinObjects.Count; i++)
                skinObjects[i].material.SetColor("_EmissionColor", (Color.Lerp(emissionColorOrig, invinsiblityEmissionColor, t)) * 50f);
            yield return null;
        }
        else if (invinsibleFlashTimer >= invinsibleFlashTime)
        {
            invinsibleFlashTimer = 0;
            currFlashes++;
        }
        if(currFlashes == invinsibleFlashes)
        {
            for (int i = 0; i < skinObjects.Count; i++)
                skinObjects[i].material.SetColor("_EmissionColor", emissionColorOrig);
            invinsibleCooldownTimer = 0;
            currFlashes = 0;
            isInvinsible = false;
        }
    }

    IEnumerator twistSword()
    {
        rotTimer += Time.deltaTime;
        isInSpecial = true;

        if (rotTimer < rotTime && !isSpinning)
            Sword.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(-45, 0, 0), Quaternion.Euler(-45, 0, 105), rotTimer * 2);
        else if (rotTimer < rotTime && isSpinning)
            Sword.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(-45, 0, 105), Quaternion.Euler(-45, 0, 0), rotTimer * 2);
        else if (rotTimer >= rotTime)
        {
            rotTimer = 0;
            if (isSpinning)
            {
                isSpinning = false;
                isAttacking = false;
            }
            else isSpinning = true;
        }

        yield return null;
    }
}
