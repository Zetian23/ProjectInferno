using UnityEngine;

public class LightTorch : MonoBehaviour, IBurnable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject firePos;
    [SerializeField] GameObject fire;

    public void melt()
    {
        Instantiate(fire, firePos.transform.position, firePos.transform.rotation);
    }
}
