using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel <3

public class sinEnemy : Enemy
{
    [SerializeField] protected float rotTime;                   
    [SerializeField] protected float xRotAngle;                 
    [SerializeField] GameObject weaponPos;
    [SerializeField] protected GameObject projectile;                        
    [SerializeField] protected List<Renderer> skinObjects;

    protected Color emissionColorOrig;

    public bool isInvinsible;
    public bool weakSpotHit;
    public bool isLust;
    public bool isKilled;
    public GameObject portal;
    protected float rotTimer;
    protected bool isSpinning;
    protected bool isInSpecial;
    protected bool isLowerred;
    protected bool isAttacking;

    public void InitVar() 
    {
        if (SavedDataManager.instance.getData().bossDefeated[gamemanager.instance.currBoss])
            Destroy(gameObject);
        else
        {
            portal = FindAnyObjectByType<LevelChange>().gameObject;
            portal.SetActive(false);
            gamemanager.instance.bossUI.SetActive(true);
            gamemanager.instance.SetPhase(1);
            startSpeed = agent.speed;
            isInvinsible = false;
            colorOrg = skinObjects[0].material.color;
            emissionColorOrig = skinObjects[0].material.GetColor("_EmissionColor");
            attackTimer = 0;
            HPOrig = HP;
            stoppingDistOrig = agent.stoppingDistance;
        }
    }

    public override void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
    }

    public override void takeDamage(int amount)
    {
        if (!isInvinsible) {
            if (HP > 0)
            {
                HP -= amount;
                StartCoroutine(flashDamage());
                updateBossUI();
            }
            if (HP <= 0)
            {
                portal.SetActive(true);
                Destroy(gameObject);
                gamemanager.instance.bossUI.SetActive(false);
                SavedDataManager.instance.getData().bossDefeated[gamemanager.instance.currBoss] = true;
            }
        }
    }

    public override IEnumerator flashDamage()
    {
        for (int i = 0; i < skinObjects.Count; i++)
            skinObjects[i].material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < skinObjects.Count; i++)
            skinObjects[i].material.SetColor("_EmissionColor", emissionColorOrig);
    }

    protected void checkHealth(int phase1HealthMin, int phase2HealthMin)
    {
        if (HP < phase1HealthMin) {
            gamemanager.instance.SetPhase(2);
            updateBossUI();
        }
        if (HP < phase2HealthMin)
        {
            gamemanager.instance.SetPhase(3);
            updateBossUI();
        }
    }

    public virtual void updateBossUI()
    {
        if (gamemanager.instance.GetPhase() == 1)
        {
            gamemanager.instance.bossHPBar[0].fillAmount = (float)HP / HPOrig;
            gamemanager.instance.bossHealthUI[0].SetActive(true);
            gamemanager.instance.bossHealthUI[1].SetActive(false);
            gamemanager.instance.bossHealthUI[2].SetActive(false);
        }
        if (gamemanager.instance.GetPhase() == 2)
        {
            gamemanager.instance.bossHPBar[1].fillAmount = (float)HP / HPOrig;
            gamemanager.instance.bossHealthUI[0].SetActive(false);
            gamemanager.instance.bossHealthUI[1].SetActive(true);
        }
        if (gamemanager.instance.GetPhase() == 3)
        {
            gamemanager.instance.bossHPBar[2].fillAmount = (float)HP / HPOrig;
            gamemanager.instance.bossHealthUI[1].SetActive(false);
            gamemanager.instance.bossHealthUI[2].SetActive(true);
        }
    }

    protected IEnumerator swingWeapon()
    {
        rotTimer += Time.deltaTime;

        if (rotTimer < rotTime && !isLowerred)
            weaponPos.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(60, 0, 0), rotTimer * 2);
        else if (rotTimer < rotTime && isLowerred)
            weaponPos.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(xRotAngle, 0, 0), Quaternion.Euler(0, 0, 0), rotTimer * 2);
        else if (rotTimer >= rotTime)
        {
            rotTimer = 0;
            if (isLowerred)
            {
                if (!isSpinning && gamemanager.instance.currBoss == 1) isInSpecial = false;
                isLowerred = false;
                isAttacking = false;
            }
            else isLowerred = true;
        }
        yield return null;
    }

    protected override void meleeAttack()
    {
        attackTimer = 0;

        RaycastHit hit;
        if (!isLowerred)
            StartCoroutine(swingWeapon());
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer)
            && isLowerred && rotTimer == 0 && gamemanager.instance.currBoss != 2)
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(attackDamage);
            }
        }
        else if (isLowerred && rotTimer == 0 && gamemanager.instance.currBoss == 2)
        {
            Instantiate(shockwave, shockwavePos.position, Quaternion.identity);
        }
        if (isLowerred) StartCoroutine(swingWeapon());
    }

    protected virtual void rangedAttack()
    {
        Vector3 playerUp = gamemanager.instance.player.transform.position - attackPos.position;
        Vector3 playerUpward = new Vector3(playerUp.x, playerUp.y - Vector3.Angle(playerUp, attackPos.up), playerUp.z);
        Instantiate(projectile, attackPos.position, Quaternion.LookRotation(playerUp, attackPos.up));
        isAttacking = false;
        attackTimer = 0;
    }

    public override void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer) && !isAttacking)
        {
            isAttacking = true;
        }
    }
}