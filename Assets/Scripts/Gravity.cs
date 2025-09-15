using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class Gravity : MonoBehaviour
{
    
    Rigidbody Player;

    Vector2 rotation = Vector2.zero;

    float VelocityChangeLimit = 10.0f;

    void Update()
    {
        Player = GetComponent<Rigidbody>();
        Player.useGravity = false;
        Player.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
    }

   

    
}
