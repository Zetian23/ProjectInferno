using UnityEngine;
using System.Collections;
public class LightningBolt : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] int chargeValue;

    [SerializeField] GameObject chainArea;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, destroyTime);
        rb.linearVelocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.CompareTag("Enemy"))
        {
            Instantiate(chainArea, transform.position, Quaternion.identity);
        }

        IChargable burn = other.GetComponent<IChargable>();

        if (burn != null)
        {
            burn.charge(chargeValue);
        }

        Destroy(gameObject);    
    }
}