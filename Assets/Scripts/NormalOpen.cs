using UnityEngine;

public class NormalOpen : MonoBehaviour
{
    
    public Animator obj;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.C))
        {
            obj.SetBool("Opening", true);
        }
    }

}
