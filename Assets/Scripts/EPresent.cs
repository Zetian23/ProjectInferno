using UnityEngine;

public class EPresent : MonoBehaviour
{
    [SerializeField] GameObject detector;
    public bool triggered = false;
    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")){
            detector.SetActive(true);
            triggered = true;
        }
    }
}
