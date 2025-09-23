using UnityEngine;

public class powerPickUp : MonoBehaviour
{
    [SerializeField] int powerID;

    void Start()
    {
        gameData data = SavedDataManager.instance.getData();

        if (data.powers[powerID] && gameObject != null)
            Destroy(gameObject);
    }

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
