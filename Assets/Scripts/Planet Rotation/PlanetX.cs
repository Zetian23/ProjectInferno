using UnityEngine;

public class PlanetX : MonoBehaviour
{
    public float rotationSpeed = 10f;
    

    

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rotationSpeed = 12;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            rotationSpeed = 10;
        }

      

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        }
    }
}
