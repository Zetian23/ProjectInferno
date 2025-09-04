using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code written by brady (Movement-wise)
public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    //base stats
    [SerializeField] int HPMax;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] weaponStats startingWeapon;
    //Range Weapon
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    //Melee Weapon
    [SerializeField] int hitDamage;
    [SerializeField] float hitRate;
    [SerializeField] int hitDist;

    //Weapon Model and Skin
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject meleeModel;
    [SerializeField] GameObject rangeModel;

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
    [SerializeField] float slothSpeedReduction;
    [SerializeField] float gluttonyHealthMod;
    [SerializeField] float wrathDamageMult;
    [SerializeField] float PrideSpeedAdd;
    [SerializeField] float envyHealPercent;

    //Leveling
    int level;
    [SerializeField] int expReqOrig;
    [SerializeField] int expReqScaling;
    int EXP;
    int expReq;
    [SerializeField] int maxHPLevelUp;
    [SerializeField] float DamageLevelUp;

    Vector3 moveDirection;
    Vector3 dashDirection;
    Vector3 playerVelocity;

    float shootTimer;
    float dashTimer;
    float activeDashTimer;

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
        getWeaponStat(startingWeapon);
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();

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


        if (Input.GetButton("Fire1") && shootTimer >= shootRate && weaponList.Count > 0 && weaponList[weaponListpos].ammoCur > 0)
        {
            shoot();
        }

        if (Input.GetButton("Fire2") && shootTimer >= hitRate)
        {
            melee();
        }

        selectWeapon();

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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

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
            }
        }
    }

    void melee()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, hitDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                //Wrath
                if (hasWrath)
                {
                    dmg.takeDamage((int)(hitDamage * wrathDamageMult));
                }
                else
                {
                    dmg.takeDamage(hitDamage);
                }

                //Envy
                if (hasEnvy)
                {
                    takeDamage((int)(hitDamage * envyHealPercent));
                }
            }
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
        changeWeapon();

    }

    void changeWeapon()
    {
        shootDamage = weaponList[weaponListpos].shootDamage;
        shootDist = weaponList[weaponListpos].shootDist;
        shootRate = weaponList[weaponListpos].shootRate;
        hitDamage = weaponList[weaponListpos].meleeDamage;
        hitDist = weaponList[weaponListpos].meleeDist;
        hitRate = weaponList[weaponListpos].meleeRate;

        meleeModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListpos].meleeModel.GetComponent<MeshFilter>().sharedMesh;
        meleeModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListpos].meleeModel.GetComponent<MeshRenderer>().sharedMaterial;
        rangeModel.GetComponent<MeshFilter>().sharedMesh = weaponList[weaponListpos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        rangeModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[weaponListpos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && weaponListpos < weaponList.Count - 1)
        {
            weaponListpos++;
            changeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && weaponListpos > 0)
        {
            weaponListpos--;
            changeWeapon();
        }
    }
}
