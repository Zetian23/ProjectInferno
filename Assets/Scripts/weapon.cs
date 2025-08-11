using JetBrains.Annotations;
using UnityEngine;
// Code written by Alex
public class Weapon : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    enum meleeWeapon { sword, spear, hammer }
    enum rangeWeapon { pistol, shotgun, AR }
    [SerializeField] meleeWeapon mw;
    [SerializeField] rangeWeapon rw;
    int damageAmount, attackRange, ammo;
    float attackSpeed;
    bool AOE;
    bool weaponType;

    void Equip(meleeWeapon emw, rangeWeapon erw)
    {
        mw = emw;
        ChangeMelee(emw);
        rw = erw;
        ChangeRange(erw);
    }

    void Start()
    {
        weaponType = true;
        mw = meleeWeapon.sword;
        rw = rangeWeapon.pistol;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            weaponType = true;
        }

        if (Input.GetButton("Fire2"))
        {
            weaponType = false;
        }
    }

    void ChangeMelee(meleeWeapon cmw)
    {
        if (cmw == meleeWeapon.sword)
        {
            damageAmount = 7;
            attackRange = 5;
            AOE = false;
            attackSpeed = 1;
        }
        else if (cmw == meleeWeapon.spear)
        {
            damageAmount = 10;
            attackRange = 7;
            AOE = false;
            attackSpeed = 0.5f;
        }
        else
        {
            damageAmount = 5;
            attackRange = 4;
            AOE = true;
            attackSpeed = 1.5f;
        }
    }

    void ChangeRange(rangeWeapon crw)
    {
        if (crw == rangeWeapon.pistol)
        {
            damageAmount = 7;
            attackRange = 15;
            AOE = false;
            attackSpeed = 0.5f;
            ammo = 10;
        }
        else if (crw == rangeWeapon.AR)
        {
            damageAmount = 10;
            attackRange = 20;
            AOE = false;
            attackSpeed = 0.3f;
            ammo = 30;
        }
        else
        {
            damageAmount = 3;
            attackRange = 12;
            AOE = true;
            attackSpeed = 2;
            ammo = 5;
        }
    }
}