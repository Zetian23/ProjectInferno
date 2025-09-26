using UnityEngine;

public class NormalOpen : MonoBehaviour
{
    
    public Animator obj;
    bool opened = false;
    [SerializeField] GameObject potion;


    private void Start()
    {
        if (potion != null)
        {
            potion.SetActive(false);
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
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.C))
        {
            obj.SetBool("Opening", true);
            opened = true;
        }
    }

}
