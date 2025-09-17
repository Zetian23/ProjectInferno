using UnityEngine;

public class Planet : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public Transform cameraTransform;


    /*[SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] float dashTime;
    [SerializeField] float dashRate;
    [SerializeField] int dashSpeed;
    [SerializeField] int dashIFrames;
    float dashTimer;
    bool isDashing;
    bool hasAirDashed;
    float activeDashTimer;*/
    private void OnTriggerStay(Collider other)
    {
        /*dashTimer += Time.deltaTime;

        if (Input.GetButtonDown("Dash") && dashTimer >= dashRate && !hasAirDashed)
        {
            dashTimer = 0;

            if (!controller.isGrounded)
            {
                hasAirDashed = true;
            }

            activeDashTimer = 0;
            isDashing = true;
            dashDirection = moveDirection;
        }

        if (isDashing && activeDashTimer <= dashTime)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            activeDashTimer += Time.deltaTime;
        }*/



        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rotationSpeed = 12;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            rotationSpeed = 10;
        }

       // float cameraYRotation = cameraTransform.eulerAngles.y;
        
       // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cameraYRotation, transform.rotation.eulerAngles.z);

        //float xaxis = transform.rotation.eulerAngles.x;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime, Space.World);
        }
       // transform.rotation = Quaternion.Euler(xaxis, cameraYRotation, transform.rotation.eulerAngles.z);


       // float zaxis = transform.rotation.eulerAngles.z;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.left, rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime, Space.World);
        }
       // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cameraYRotation, zaxis);

    }
}
