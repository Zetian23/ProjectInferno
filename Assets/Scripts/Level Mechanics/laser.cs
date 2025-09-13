using UnityEngine;
using UnityEngine.UIElements;
// Code Written By Nathaniel King <3

public class laser : MonoBehaviour
{
    [SerializeField] GameObject laserBeam;
    [SerializeField] bool isPatterned;
    [SerializeField] float visibleRate;
    [SerializeField] float beamSize;

    float visibleTimer;

    void Start()
    {
        visibleTimer = 0f;
        laserBeam.transform.localScale = new Vector3(laserBeam.transform.localScale.x, beamSize, laserBeam.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatterned)
        {
            visibleTimer += Time.deltaTime;
            if (visibleTimer >= visibleRate && laserBeam.activeInHierarchy)
            {
                laserBeam.SetActive(false);
                visibleTimer = 0f;
            }
            else if (visibleTimer >= visibleRate && !laserBeam.activeInHierarchy)
            {
                laserBeam.SetActive(true);
                visibleTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lightning"))
        {
            isPatterned = false;
            laserBeam.SetActive(false);
        }
    }
}
