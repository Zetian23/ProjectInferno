using UnityEngine;

public class PlatformDebugChecker : MonoBehaviour
{
    public GameObject player;
    public CharacterController playerController;

    void Start()
    {
        // Find all objects tagged "Platform"
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (var platform in platforms)
        {
            var ai = platform.GetComponent<platformAI>();
            if (ai == null)
            {
                Debug.LogWarning($"Platform '{platform.name}' is missing platformAI script!");
            }
            else
            {
                Debug.Log($"Platform '{platform.name}' has platformAI attached.");
            }
        }

        // Find player and controller
        if (player == null)
            player = GameObject.FindWithTag("Player");
        if (player != null)
            playerController = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if player is grounded and on a platform
        if (playerController != null && playerController.isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, Vector3.down, out hit, 2f))
            {
                if (hit.collider.CompareTag("Platform"))
                {
                    var platformAI = hit.collider.GetComponent<platformAI>();
                    if (platformAI != null)
                    {
                        Debug.Log($"Player is on platform '{hit.collider.name}'. Platform position: {hit.collider.transform.position}");
                    }
                    else
                    {
                        Debug.LogWarning($"Player is on '{hit.collider.name}' tagged 'Platform' but missing platformAI!");
                    }
                }
            }
        }
    }
}