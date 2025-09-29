using UnityEngine;

public class Open : MonoBehaviour, IChargable
{
    
    [SerializeField] int maxCharge = 5;
    public Animator obj;
    bool opened = false;
    [SerializeField] GameObject potion;
    [SerializeField]int currcharge;
    [SerializeField] GameObject door;
    [SerializeField] Material rend;

    
    
    void Start()
    {
        currcharge = 0;
        Renderer renderer = door.GetComponent<Renderer>();
        if (potion != null)
        {
            potion.SetActive(false);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if(currcharge >= maxCharge)
        {
            obj.SetBool("Opening", true);
            if(rend != null) door.GetComponent<Renderer>().material = rend;
            if(potion != null) potion.SetActive(true);
        }
        
    }

    public void charge(int chargeVal)
    {
        
        currcharge += chargeVal;
    }

}
