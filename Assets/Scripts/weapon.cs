using UnityEngine;
// Code written by Alex
public class Weapon : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Weapon instance;

    int damageAmount, attackRange, Damageamount, Attackrange;
    float attackSpeed, Attackspeed;
    bool AOE;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        damageAmount = 7;
        attackRange = 5;
        AOE = false;
        attackSpeed = 1;

        Damageamount = 7;
        Attackrange = 15;
        Attackspeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WeaponType()
    {
        if (Input.GetButtonDown("Sword"))
        {
            damageAmount = 7;
            attackRange = 5;
            AOE = false;
            attackSpeed = 1;

            Damageamount = 7;
            Attackrange = 15;
            Attackspeed = 0.5f;

            

        }

        if (Input.GetButton("Spear"))
        {
            damageAmount = 10;
            attackRange = 7;
            AOE = false;
            attackSpeed = 0.5f;

            Damageamount = 10;
            Attackrange = 20;
            Attackspeed = 0.3f;

            

        }
        if (Input.GetButton("Hammer"))
        {
            damageAmount = 5;
            attackRange = 4;
            AOE = true;
            attackSpeed = 1.5f;
     
            Damageamount = 3;
            Attackrange = 12;
            Attackspeed = 2;

            
        }
    }

   public void ChangeWeapon(ref int da, ref int ar, ref bool aoe, ref float ats, ref int DA, ref int AR, ref float ATS)
    {
        da = damageAmount;
        ar = attackRange;
        aoe = AOE;
        ats = attackSpeed;
        
        DA = Damageamount;
        AR = Attackrange;
        ATS = Attackspeed;
    }

   


}