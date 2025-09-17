using UnityEngine;

public class RockOpen : MonoBehaviour
{
    public GameObject rock;
    public Animator obj;

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Player")) { 
            obj.SetBool("Opening", true);
        }
    }
}
