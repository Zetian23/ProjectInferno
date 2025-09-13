using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class IceShock : MonoBehaviour
{
    [SerializeField] int destroyTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IFreezable freeze = other.GetComponent<IFreezable>();

        if (freeze != null)
        {
            freeze.freeze();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IFreezable freeze = other.GetComponent<IFreezable>();

        if (freeze != null)
        {
            freeze.unfreeze();
        }
    }
}
