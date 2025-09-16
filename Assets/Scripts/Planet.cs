using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float gravity = 9.81f;

    public void Attract(Transform body)
    {
        Vector3 targetDir = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, targetDir) * body.rotation;
        body.GetComponent<Rigidbody>().AddForce(targetDir * gravity, ForceMode.Acceleration);
        
    }
}
