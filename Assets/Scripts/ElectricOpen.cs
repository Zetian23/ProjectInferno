using UnityEngine;

public class Open : MonoBehaviour, IChargable
{
    
    [SerializeField] int maxCharge = 5;
    public Animator obj;
    [SerializeField]int currcharge;
    [SerializeField] GameObject door;
    [SerializeField] Material rend;


    void Start()
    {
        currcharge = 0;
        Renderer renderer = door.GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if(currcharge >= maxCharge)
        {
            obj.SetBool("Opening", true);
            if(rend != null) door.GetComponent<Renderer>().material = rend;
        }
    }

    public void charge(int chargeVal)
    {
        
        currcharge += chargeVal;
    }

}
