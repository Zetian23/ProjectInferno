using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform teleportTarget;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            other.transform.parent = transform;
            other.transform.position = teleportTarget.position;
            other.transform.parent = null;

        }
    }
}
