using UnityEngine;
using System.Collections;
public class LazerBeam : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

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
        if (other.isTrigger) return;

        IDamage damage = other.GetComponent<IDamage>();

        if (damage != null && other.CompareTag("Enemy"))
        {
            damage.takeDamage(7);
        }  
    }
}