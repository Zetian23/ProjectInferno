using UnityEngine;
// Code written by Alex
public class Weapon : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] int cl;

    public static Weapon instance;

    int damageAmount, attackRange, ammo;
    float attackSpeed;
    bool AOE;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WeaponType()
    {
        if (Input.GetButtonDown("Sword"))
        {
            damageAmount = 7;
            attackRange = 5;
            AOE = false;
            attackSpeed = 1;
            ammo = -1;
            cl = 1;
        }

        if (Input.GetButton("Spear"))
        {
            damageAmount = 10;
            attackRange = 7;
            AOE = false;
            attackSpeed = 0.5f;
            ammo = -1;
            cl = 2;
        }
        if (Input.GetButton("Hammer"))
        {
            damageAmount = 5;
            attackRange = 4;
            AOE = true;
            attackSpeed = 1.5f;
            ammo = -1;
            cl = 3;
        }
        if (Input.GetButton("Pistol"))
        {
            damageAmount = 7;
            attackRange = 15;
            AOE = false;
            attackSpeed = 0.5f;
            ammo = 10;
            cl = 4;
        }
        if (Input.GetButton("Shotgun"))
        {
            damageAmount = 10;
            attackRange = 20;
            AOE = false;
            attackSpeed = 0.3f;
            ammo = 30;
            cl = 5;
        }
        if (Input.GetButton("AR"))
        {
            damageAmount = 3;
            attackRange = 12;
            AOE = true;
            attackSpeed = 2;
            ammo = 5;
            cl = 6;
        }
    }

   void ChangeWeapon(ref int da, ref int ar, ref bool aoe, ref float ats, ref int am)
    {
        da = damageAmount;
        ar = attackRange;
        aoe = AOE;
        ats = attackSpeed;
        am = ammo;
    }


}