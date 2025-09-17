using UnityEngine;
using System.Collections;
public class Stone : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    [SerializeField] GameObject plate;


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

        Instantiate(plate, transform.position, Quaternion.Euler(transform.forward));

        Destroy(gameObject);
    }
}