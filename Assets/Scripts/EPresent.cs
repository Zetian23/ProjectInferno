using UnityEngine;

public class EPresent : MonoBehaviour
{
    [SerializeField] GameObject detector;
    bool triggered = false;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && triggered == false){
            detector.SetActive(true);
            triggered = true;
        }
    }
}
