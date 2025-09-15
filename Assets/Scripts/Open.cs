using UnityEngine;

public class Open : MonoBehaviour, IChargable
{
    GameObject player;
    [SerializeField] int maxCharge = 5;
    public Animator chest;
    [SerializeField]int currcharge = 0;

    public void charge(int chargeVal = 1)
    {
        Destroy(player);
        //currcharge += chargeVal;
    }
    // Update is called once per frame
    void Update()
    {
        if(currcharge >= maxCharge)
        {
            chest.SetBool("Opening", true);
        }
    }
}
