using UnityEngine;

public class StepSwitch : MonoBehaviour
{
    public bool cho;
    public GameObject objPos;
    public GameObject gameObj;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (cho == true)
            {
                Instantiate(gameObj);
                gameObj.transform.position = objPos.transform.position;
                gameObj.transform.rotation = objPos.transform.rotation;
                gameObj.transform.parent = objPos.transform;
            } else {
                Destroy(gameObj);
            }
        }
       
        Destroy(this);
    }
}
