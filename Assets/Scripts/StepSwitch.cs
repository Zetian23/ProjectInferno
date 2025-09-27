using UnityEngine;

public class StepSwitch : MonoBehaviour
{
    public bool cho;
    public Transform objPos;
    public GameObject gameObj;
    public int tri = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && tri == 0)
        {
            tri = 1;
            if (cho == true)
            {
                gameObj.transform.position = objPos.position;
                gameObj.transform.rotation = objPos.rotation;
            } else {
                Destroy(gameObj);
            }
        }
       
        Destroy(this);
    }
}
