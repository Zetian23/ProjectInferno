using UnityEngine;


public class Gravity : MonoBehaviour
{

    public Transform cameraTransform;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && cameraTransform != null)
        {
            float cameraYRotation = cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);
        }
    }
}
