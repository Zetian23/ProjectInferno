using System.Collections;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject explosion;

    [SerializeField] int speed;
    [SerializeField] int destroyTime;

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
        if (other.isTrigger || other.CompareTag("Player")) return;

        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
