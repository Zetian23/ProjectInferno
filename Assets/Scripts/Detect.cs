using System.Collections;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public bool cho;
    public Transform objPos;
    public GameObject gameObj;
    int tri = 0;
    private void OnTriggerExit(Collider other)
    {  
        if (other.CompareTag("Enemy") && tri == 0)
        {
            //tri = 1;
            if (cho == true)
            {
                gameObj.transform.position = objPos.position;
                gameObj.transform.rotation = objPos.rotation;
            }
            else
            {
                Destroy(gameObj);
            }
        }


    }

}
