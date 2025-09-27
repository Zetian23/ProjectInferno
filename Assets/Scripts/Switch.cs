using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject obj;
    public Animator animate;
   
    public bool trig = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.C))
        {
            animate.SetBool("trig", true);
            trig = true;
        }
    }
}
