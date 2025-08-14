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
        damageAmount = 10;
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
        if (Input.GetKeyDown("1"))
        {
            damageAmount = 10;
            attackRange = 10;
            AOE = false;
            attackSpeed = 1;

            Damageamount = 10;
            Attackrange = 25;
            Attackspeed = 0.5f;

            

        }

        if (Input.GetKeyDown("2"))
        {
            damageAmount = 15;
            attackRange = 15;
            AOE = false;
            attackSpeed = 1;

            Damageamount = 8;
            Attackrange = 35;
            Attackspeed = 0.3f;

            

        }
        if (Input.GetKeyDown("3"))
        {
            damageAmount = 7;
            attackRange = 10;
            AOE = true;
            attackSpeed = 1.5f;
     
            Damageamount = 5;
            Attackrange = 20;
            Attackspeed = 2;

            
        }
    }

   public void ChangeWeapon(ref int DA, ref int AR, ref bool aoe, ref float ATS, ref int da, ref int ar, ref float ats)
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