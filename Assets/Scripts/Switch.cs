using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject obj;
    public Animator animate;
    public bool cho;
    public Transform objPos;
    public GameObject gameObj;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            animate.SetBool("trig", true);
        }
        if(cho == true)
        {
            Instantiate(gameObj, objPos.position, objPos.rotation);
        } else
        {
            Destroy(gameObj);
        }
    }
}
