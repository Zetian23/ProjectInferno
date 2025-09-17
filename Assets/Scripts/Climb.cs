using System.Runtime.CompilerServices;
using UnityEngine;

public class Climb : MonoBehaviour, IBurnable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject rope;

    public void melt()
    {
        Destroy(rope);
    }
    /*private void OnTriggerEnter(Collider other)
{
   if (other.CompareTag("Player"))
   {
       other.transform.parent = transform;
       other.transform.position = rope.transform.position;
   }
}

private void OnTriggerStay(Collider other)
{
   if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.W))
   {
       other.GetComponentInChildren<Transform>().position += new Vector3(0, 2, 0);

   }
   if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.S))
   {
       other.GetComponentInChildren<Transform>().position += new Vector3(0, 2, 0);

   }
}

private void OnTriggerExit(Collider other)
{
   if (other.CompareTag("Player"))
   {
       other.transform.parent = null;

   }
}*/

}
