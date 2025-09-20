using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject obj;
    public Animator animate;
    public bool cho;
    public Transform objPos;
    public GameObject gameObj;
    bool trig = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            animate.SetBool("trig", true);
        }
        if(cho == true)
        {
            gameObj.transform.position = objPos.position;
        } else
        {
            Destroy(gameObj);
        }
    trig = true;
    }
}
