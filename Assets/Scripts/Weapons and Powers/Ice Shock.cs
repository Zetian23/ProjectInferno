using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class IceShock : MonoBehaviour
{
    [SerializeField] float destroyTime;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] Collider collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] Rigidbody rb;
    float destroyTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        destroyTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer > destroyTime - 0.1)
        {
            rb.AddForce(new Vector3(0, -9999, 0));
        }
        if(destroyTimer > destroyTime)
        {
            Destroy(gameObject);
        }
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
