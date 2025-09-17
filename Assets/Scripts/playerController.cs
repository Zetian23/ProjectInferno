using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//Code written by brady (Movement-wise)
public class playerController : MonoBehaviour, IDamage, iPickUp, ISavedData
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    //base stats
    [SerializeField] public int HPMax;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    //Range Weapon
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] ParticleSystem shootEffect;
    [SerializeField] GameObject shootPos;

    //Weapon Model and Skin
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject powerModel;

    //Dashing
    [SerializeField] float dashTime;
    [SerializeField] float dashRate;
    [SerializeField] int dashSpeed;
    [SerializeField] int dashIFrames;

    //Sins
    [SerializeField] bool hasLust;
    [SerializeField] bool hasGreed;
    [SerializeField] bool hasSloth;
    [SerializeField] bool hasGluttony;
    [SerializeField] bool hasWrath;
    [SerializeField] bool hasPride;
    [SerializeField] bool hasEnvy;

    //Sin Modifiers
    float lustTimer;
    [SerializeField] float lustRate;
    [SerializeField] float lustHealPercent;
    [SerializeField] float greedEXPMod;
    [SerializeField] public float slothSpeedReduction;
    [SerializeField] float gluttonyHealthMod;
    [SerializeField] public float wrathDamageMult;
    [SerializeField] float PrideSpeedAdd;
    [SerializeField] public float envyHealPercent;

    //Leveling
    public int level;
    [SerializeField] int expReqOrig;
    [SerializeField] int expReqScaling;
    int EXP;
    int expReq;
    [SerializeField] int maxHPLevelUp;
    [SerializeField] public float DamageLevelUp;

    //Powers
    int powerPos;
    List<bool> powerList = new();
    List<GameObject> powerModels = new();
    //Fireball
    [SerializeField] GameObject fireModel;
    [SerializeField] GameObject fireProjectile;
    [SerializeField] float fireRate;
    //Chain Lightning
    [SerializeField] GameObject lightningModel;
    [SerializeField] GameObject lightningProjectile;
    [SerializeField] float lightningRate;
    //Ice Shock
    [SerializeField] GameObject iceModel;
    [SerializeField] float iceRate;
    [SerializeField] GameObject iceZone;
    //Wind Charge
    [SerializeField] GameObject windModel;
    [SerializeField] float windRate;
    [SerializeField] int windSpeed;
    [SerializeField] GameObject windBox;
    //Stone Model
    [SerializeField] GameObject stoneModel;
    [SerializeField] GameObject stone;
    [SerializeField] float stoneRate;

    Vector3 moveDirection;
    Vector3 dashDirection;
    Vector3 playerVelocity;

    float shootTimer;
    float dashTimer;
    float activeDashTimer;
    float powerTimer;

    int jumpCount;
    int HP;
    int weaponListpos;

    bool isDashing;
    bool hasAirDashed;
    bool hasPrideAdded = false;
    bool hasGluttAdded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HP = HPMax;
        level = 1;
        EXP = 0;
        expReq = expReqOrig;

        for (int i = 0; i < 5; i++)
        {
            powerList.Add(false);
        }

        powerModels.Add(fireModel);
        powerModels.Add(lightningModel);
        powerModels.Add(iceModel);
        powerModels.Add(windModel);
        powerModels.Add(stoneModel);

        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();

        //Debug.Log(powerPos);

        //Lust
        if (hasLust)
        {
            lustTimer += Time.deltaTime;

            if(lustTimer >= lustRate)
            {
                takeDamage((int)(HPMax * lustHealPercent * -1));
                lustTimer = 0;
            }
        }
    }

    void movement()
    {
        shootTimer += Time.deltaTime;
        dashTimer += Time.deltaTime;
        powerTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            jumpCount = 0;
            hasAirDashed = false;
            playerVelocity = Vector3.zero;
        }

        moveDirection = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDirection * speed * Time.deltaTime);

        jump();

        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;


        if (Input.GetButton("Fire1") && weaponList.Count != 0 && shootTimer >= shootRate && weaponList[weaponListpos].ammoCur != 0)
        {
            shoot();
        }

        if (Input.GetButton("Fire2") && powerList[0])
        {
            power();
        }

        if (powerList[0])
        {
            selectPower();
        }

        //reload
        if (Input.GetButton("Reload") && weaponList.Count != 0 && weaponList[weaponListpos].ammoCur != weaponList[weaponListpos].ammoMax)
        {
            reload();
        }

        //Dash function
        if (Input.GetButtonDown("Dash") && dashTimer >= dashRate && !hasAirDashed)
        {
            dashTimer = 0;

            if (!controller.isGrounded)
            {
                hasAirDashed = true;
            }

            activeDashTimer = 0;
            isDashing = true;
            dashDirection = moveDirection;
        }

        if (isDashing && activeDashTimer <= dashTime)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            activeDashTimer += Time.deltaTime;
        }

        //Pride
        if(!hasPrideAdded && hasPride)
        {
            speed += PrideSpeedAdd;
            hasPrideAdded = true;
        }

        //Gluttony
        if(!hasGluttAdded && hasGluttony)
        {
            HPMax = (int)(HPMax * gluttonyHealthMod);
            maxHPLevelUp = (int)(maxHPLevelUp * gluttonyHealthMod);
            hasGluttAdded = true;
        }
    }

    void reload()
    {
        weaponList[weaponListpos].ammoCur = weaponList[weaponListpos].ammoMax;
        updateGunUI();
    }

    public virtual void gainEXP(int expGained)
    {
        EXP += expGained;

        //Greed
        if (hasGreed)
        {
            EXP += (int)(expGained * greedEXPMod);
        }

        if (EXP >= expReq)
        {
            levelUp();
        }

        updatePlayerUI();
    }

    void levelUp()
    {
        level++;
        EXP -= expReq;
        expReq += expReqScaling;

        HPMax += maxHPLevelUp;
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void shoot()
    {
        shootTimer = 0;
        weaponList[weaponListpos].ammoCur--;
        updateGunUI();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            //Debug.Log(hit.collider.name);

            Instantiate(shootEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                //Wrath
                if (hasWrath)
                {
                    dmg.takeDamage((int)(shootDamage * wrathDamageMult * (DamageLevelUp * level + 1)));
                }
                else
                {
                    dmg.takeDamage((int)(shootDamage * (DamageLevelUp * level + 1)));
                }

                //Sloth
                if (hasSloth)
                {
                    dmg.slothSlow(slothSpeedReduction);
                }

                if (hasEnvy)
                {
                    takeDamage((int)(shootDamage * envyHealPercent));
                }
            }
        }
    }

    void power()
    {
       
        switch (powerPos)
        {
            case 0:
                if (powerTimer >= fireRate)
                {
                    Instantiate(fireProjectile, shootPos.transform.position, Camera.main.transform.rotation);
                    powerTimer = 0;
                }
                break;
            case 1:
                
                if (powerTimer >= lightningRate)
                {
                    Instantiate(lightningProjectile, shootPos.transform.position, Camera.main.transform.rotation);
                    powerTimer = 0;
                }
                break;
            case 2:
                if (powerTimer >= iceRate)
                {
                    Instantiate(iceZone, Camera.main.transform.position, Quaternion.identity);
                    powerTimer = 0;
                }
                break;
            case 3:
                if (powerTimer >= windRate)
                {
                    Instantiate(windBox, transform.position, Quaternion.identity);
                    playerVelocity.y = windSpeed;
                    powerTimer = 0;
                }
                break;
            case 4:
                if (powerTimer >= stoneRate)
                {
                    Instantiate(stone, Camera.main.transform.position, Camera.main.transform.rotation);
                    powerTimer = 0;
                }
                break;
        }
    }

    public void takeDamage(int amount)
    {
        if (amount < 0)
            StartCoroutine(healingFlash());
        else
            StartCoroutine(damageFlash());
        HP = HP - amount;

        if (HP > HPMax) 
        {
            HP = HPMax;
        }

        updatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }



    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPMax;
        gamemanager.instance.playerEXPBar.fillAmount = (float)EXP / expReq;
    }

    public void updateGunUI()
    {
        gamemanager.instance.playerAmmoCur = weaponList[weaponListpos].ammoCur;
        gamemanager.instance.playerAmmoMax = weaponList[weaponListpos].ammoMax;
    }


    IEnumerator damageFlash()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }

    IEnumerator healingFlash() //-N 
    {
        gamemanager.instance.playerHealFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerHealFlash.SetActive(false);
    }

    IEnumerator levelUpFlash() //-N 
    {
        gamemanager.instance.playerLevelUPFlash.SetActive(true);
        yield return new WaitForSeconds(2);
        gamemanager.instance.playerLevelUPFlash.SetActive(false);
    }

    public void slothSlow(float percent)
    {
        throw new System.NotImplementedException();
    }

    public void getWeaponStat(weaponStats weapon)
    {
        weaponList.Add(weapon);
        weaponListpos = weaponList.Count - 1;
        updateGunUI();
        changeWeapon();
    }

    public List<bool> getPlayersUpgrade()
    {
        return new List<bool>() { hasSloth, hasWrath, hasGluttony, hasEnvy, hasLust, hasGreed, hasPride };
    }

    void changeWeapon()
    {
        shootDamage = weaponList[weaponListpos].shootDamage;
        shootDist = weaponList[weaponListpos].shootDist;
        shootRate = weaponList[weaponListpos].shootRate;
        shootEffect = weaponList[weaponListpos].shootEffect;
        
        

        gunModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListpos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListpos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectPower()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            powerPos--;

            if (powerPos <= -1)
            {
                powerPos = 4;
            }

            while (!powerList[powerPos])
            {
                powerPos--;
                if (powerPos <= -1)
                {
                    powerPos = 4;
                }
            }
            equipPower();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            powerPos++;

            if (powerPos >= 5)
            {
                powerPos = 0;
            }

            while (!powerList[powerPos])
            {
                powerPos++;
                if(powerPos >= 5)
                {
                    powerPos = 0;
                }
            }
            equipPower();
        }
    }

    void equipPower()
    {
        gamemanager.instance.DisplayPowerIcon(powerPos);
        powerModel.GetComponent<MeshFilter>().sharedMesh = powerModels[powerPos].GetComponent<MeshFilter>().sharedMesh;
        powerModel.GetComponent<MeshRenderer>().sharedMaterial = powerModels[powerPos].GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void getPower(int powerID)
    {
        powerList[powerID] = true;
        powerPos = powerID;
        gamemanager.instance.DisplayPowerIcon(powerPos);
        equipPower();

    }

    public List<weaponStats> getWeaponList()
    {
        return weaponList;
    }

    public int getWeaponIndex()
    {
        return weaponListpos;
    }

    public void loadData(gameData data)
    {
        powerList = data.powers;
        weaponList = data.weapons;
        level = data.Level;
        changeWeapon();
    }

    public void saveData(ref gameData data)
    {
        data.powers = powerList;
        data.weapons = weaponList;
        data.Level = level;
    }
}
