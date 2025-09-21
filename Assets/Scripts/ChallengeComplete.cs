using UnityEngine;

public class ChallengeComplete : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int SwitchCount = 0;
    public GameObject Switch1;
    public GameObject Switch2;
    public GameObject Switch3;
    public GameObject Switch4;
    public GameObject act;
    public int count = 0;
    bool callOne = true;
    bool callTwo = true;
    bool callThree = true;
    bool callFour = true;


    // Update is called once per frame
    void Update()
    {
        if (callOne)
        {
            Switch one = Switch1.GetComponent<Switch>();
            bool oneActive = one.trig;
            if (oneActive) { 
                count++;
                callOne = false;
            }
            
        }
        if (callTwo)
        {
            if (Switch2 != null)
            {
                Switch two = Switch2.GetComponent<Switch>();
                bool twoActive = two.trig;
                if (twoActive) {
                    count++;
                    callTwo = false;
                }
            }
            
        }
        if (callThree)
        {
            if (Switch3 != null)
            {
                Switch three = Switch3.GetComponent<Switch>();
                bool threeActive = three.trig;
                if (threeActive) { 
                    count++;
                    callThree = false;
                }
            }
           
        }

        if (callFour)
        {
            if (Switch4 != null)
            {
                Switch four = Switch4.GetComponent<Switch>();
                bool fourActive = four.trig;
                if (fourActive) { 
                    count++;
                    callFour = false;
                }
            }
            
        }

        

        if (count == SwitchCount)
        {
            act.SetActive(true);
        }    
    }
}
