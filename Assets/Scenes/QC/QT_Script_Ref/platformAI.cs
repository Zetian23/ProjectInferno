using UnityEngine;

public class platformAI : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float[] waypointDistances; // x-axis distances from origin
    [SerializeField] private bool useRicochet = false; // modular option
    [SerializeField] private Vector3 ricochetDirection = Vector3.right; // ricochet direction
    private int currentWaypointIndex = 0;
    private float waitTime = 1f;
    private float waitTimer = 0f;
    private bool isPaused = false;
    private Vector3 origin;
    private Rigidbody rb;

    void Awake()
    { 
        origin = transform.position; // Set origin to initial position
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = !useRicochet; // Kinematic for waypoint, non-kinematic for ricochet

        if (useRicochet)
        {
            // Ricochet handled in FixedUpdate
        }
        else if (waypointDistances != null && waypointDistances.Length > 0)
        {
            rb.MovePosition(origin + new Vector3(waypointDistances[0], 0, 0));
            currentWaypointIndex = 0;
        }
    }

    void Update()
    {
        if (isPaused)
            return;

        if (useRicochet)
            return; // Ricochet handled in FixedUpdate

        if (waypointDistances == null || waypointDistances.Length == 0)
            return;

        Vector3 targetPosition = origin + new Vector3(waypointDistances[currentWaypointIndex], 0, 0);
        Vector3 currentPosition = transform.position;
        Vector3 direction = targetPosition - currentPosition;
        float distance = direction.magnitude;

        if (distance < 0.01f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypointDistances.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
        }
        else
        {
            Vector3 moveDir = direction.normalized;
            Vector3 newPosition = currentPosition + moveDir * speed * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void FixedUpdate()
    {
        if (useRicochet && rb != null)
        {
            rb.linearVelocity = ricochetDirection.normalized * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (useRicochet && collision.gameObject.CompareTag("Wall"))
        {
            ricochetDirection = -ricochetDirection;
        }
    }
}
