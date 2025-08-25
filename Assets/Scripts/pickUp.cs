using UnityEngine;

public class pickUp : MonoBehaviour
{
    [SerializeField] weaponStat weapon;

    private void OnTriggerEnter(Collider other)
    {
        iPickUp pickupable = other.GetComponent<iPickUp>();
        if (pickupable != null)
        {
            pickupable.getWeaponStat(weapon);
            weapon.amoCur = weapon.amoMax;
            Destroy(gameObject);
        }
    }
}
