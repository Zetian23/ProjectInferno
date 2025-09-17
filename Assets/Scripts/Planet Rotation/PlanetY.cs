using UnityEngine;

public class PlanetY : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        }


    }
}
