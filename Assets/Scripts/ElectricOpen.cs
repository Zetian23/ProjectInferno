using UnityEngine;

public class Open : MonoBehaviour, IChargable
{
    
    [SerializeField] int maxCharge = 5;
    public Animator obj;
    [SerializeField]int currcharge;

    void Start()
    {
        currcharge = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
        if(currcharge >= maxCharge)
        {
            obj.SetBool("Opening", true);
        }
    }

    public void charge(int chargeVal)
    {
        
        currcharge += chargeVal;
    }

}
