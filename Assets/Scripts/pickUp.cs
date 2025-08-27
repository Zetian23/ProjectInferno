using UnityEngine;

public class pickUp : MonoBehaviour
{
    [SerializeField] weaponStats weapon;

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
