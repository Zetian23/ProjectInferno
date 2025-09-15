using UnityEngine;

public class shockwaveController : MonoBehaviour
{
    [SerializeField] float expandSpeed;
    [SerializeField] float maxRadius;
    [SerializeField] int damage;
    [SerializeField] float lifeTime;

    private SphereCollider sphere;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sphere = GetComponent<SphereCollider>();

        if(sphere == null )
        {
            sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
        }

        sphere.radius = 0.1f;

        Destroy(gameObject, damage);
    }

    // Update is called once per frame
    void Update()
    {
        if(sphere.radius < maxRadius)
        {
            sphere.radius += expandSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Shockwave hit Player!");
            other.GetComponent<IDamage>().takeDamage(damage);
        }
    }
}
