using Mono.Cecil.Cil;
using UnityEngine;

public class WindBurst : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject);
    }
}
