using UnityEngine;

public class ricochetAI : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Vector3 direction = Vector3.right; // Set default direction
    [SerializeField] private float[] waypointDistances; // boundaries for ricochet
    private Rigidbody rb;
    private Vector3 origin;
    private float minX, maxX;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
        }
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        origin = transform.position;
        if (waypointDistances != null && waypointDistances.Length > 0)
        {
            // Clamp and validate boundaries
            minX = Mathf.Min(waypointDistances);
            maxX = Mathf.Max(waypointDistances);
            if (minX == maxX)
            {
                // Avoid overlapping boundaries
                maxX = minX + 1f;
            }
            transform.position = origin + new Vector3(waypointDistances[0], 0, 0);
        }
        else
        {
            minX = -10f;
            maxX = 10f;
        }
    }

    void Update()
    {
        rb.linearVelocity = direction.normalized * speed;
        float xPos = transform.position.x - origin.x;
        // If wall starts at boundary and direction points outward, reverse immediately
        if ((xPos <= minX && direction.x < 0) || (xPos >= maxX && direction.x > 0))
        {
            direction = -direction;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = Vector3.Reflect(direction, collision.contacts[0].normal);
        }
    }
}
