using UnityEngine;

public class powerPickUp : MonoBehaviour
{
    [SerializeField] int powerID;

    private void OnTriggerEnter(Collider other)
    {
        iPickUp pickupable = other.GetComponent<iPickUp>();

        if (pickupable != null)
        {
            pickupable.getPower(powerID);
            Destroy(gameObject);
        }
    }
}
