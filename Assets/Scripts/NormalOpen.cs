using UnityEngine;

public class NormalOpen : MonoBehaviour
{
    
    public Animator obj;
    bool opened = false;
    bool gone = true;
    [SerializeField] GameObject potion;
    [SerializeField] GameObject seal;


    private void Start()
    {
        if (potion != null)
        {
            potion.SetActive(false);
        }
        if(seal != null)
        {
            gone=false;
        }
    }
    private void Update()
    {
        if (opened && potion != null)
        {
            potion.SetActive(true);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Interact") && seal == false)
        {
            obj.SetBool("Opening", true);
            opened = true;
        }
    }

}
