using UnityEngine;

public class Open : MonoBehaviour, IChargable
{

    [SerializeField] int maxCharge = 5;
    int currcharge = 0;
    bool open = false;

    public void charge(int chargeVal)
    {
        currcharge += chargeVal;
    }
    // Update is called once per frame
    void Update()
    {
        if(currcharge >= maxCharge)
        {
            open = true;
        }
    }
}
