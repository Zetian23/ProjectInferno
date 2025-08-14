using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    //Second Weapon
    [SerializeField] int sDamage;
    [SerializeField] float sRate;
    [SerializeField] int sDist;
    

    [SerializeField] bool Aoe;

    [SerializeField] float dashTime;
    [SerializeField] float dashRate;
    [SerializeField] int dashSpeed;
    [SerializeField] int dashIFrames;

    Vector3 moveDirection;
    Vector3 dashDirection;
    Vector3 playerVelocity;

    float shootTimer;
    float dashTimer;
    float activeDashTimer;

    int jumpCount;
    int HPOrig;

    bool isSprinting;
    bool isDashing;
    bool hasAirDashed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();

        Weapon.instance.ChangeWeapon(ref shootDamage, ref shootDist, ref Aoe, ref shootRate, ref sDamage, ref sDist, ref sRate);

    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
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


        //Weapon Change
        Weapon.instance.WeaponType();
        Weapon.instance.ChangeWeapon(ref shootDamage, ref shootDist, ref Aoe, ref shootRate, ref sDamage, ref sDist, ref sRate);

        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            shoot();
        }

        if (Input.GetButton("Fire2") && shootTimer >= sRate)
        {
            shootSecond();
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
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    // Second weapon function
    void shootSecond()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, sDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(sDamage);
            }
        }
    }


    public void takeDamage(int amount)
    {
        HP -= amount;

        updatePlayerUI();

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator damageFlash()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }
}
