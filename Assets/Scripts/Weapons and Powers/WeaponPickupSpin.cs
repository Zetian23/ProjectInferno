using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class WeaponPickupSpin : MonoBehaviour
{

    [SerializeField] float changeInHeight;
    [SerializeField] float floatTime;
    [SerializeField] float spinSpeed;
    [SerializeField] float clearance;

    bool up = true;
    Vector3 startingPos;
    Vector3 vel = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward + Vector3.up, spinSpeed * Time.deltaTime);

        if (up)
        {
            transform.position = Vector3.SmoothDamp(transform.position, startingPos + (changeInHeight * Vector3.up), ref vel, floatTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, startingPos - (changeInHeight * Vector3.up), ref vel, floatTime);
        }

        if (transform.position.y - startingPos.y >= changeInHeight - clearance)
        { 
            up = false;
        }
        else if (transform.position.y - startingPos.y <= clearance - changeInHeight)
        {
            up = true;
        }
    }
}
