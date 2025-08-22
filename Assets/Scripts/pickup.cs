using UnityEngine;

public class pickup : MonoBehaviour
{

    [SerializeField] Weapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pickupable = GetComponent<IPickup>();
        if (pickupable != null)
        {
            //after the weapon stuff is rewritten for modularity
            //use this to assign things
            Destroy(gameObject);
        }
    }


}


