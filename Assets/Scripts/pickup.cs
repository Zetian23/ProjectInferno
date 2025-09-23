using UnityEngine;

public class pickUp : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

    void Start()
    {
        gameData data = SavedDataManager.instance.getData();

        for (int i = 0; i < data.weapons.Length; i++)
        {
            if (weapon == data.weapons[i] && gameObject != null)
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        iPickUp pickupable = other.GetComponent<iPickUp>();

        if (pickupable != null)
        {
            pickupable.getWeaponStat(weapon);
            weapon.ammoCur = weapon.ammoMax;
            Destroy(gameObject);
        }
    }
}
