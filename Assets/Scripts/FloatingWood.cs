using UnityEngine;
using System.Collections;

public class FloatingWood : MonoBehaviour, IFreezable
{
    
    [SerializeField] int speed;
    int OrigSpeed;
    [SerializeField] Transform platform;
    [SerializeField] Transform startingpos;
    [SerializeField] Transform destination;
    [SerializeField] Transform midOne;
    [SerializeField] Transform midTwo;
    [SerializeField] GameObject wood;
    [SerializeField] bool midpoint1Reached = false;
    [SerializeField] bool midpoint2Reached = false;
    


    Vector3 startingPos;
    bool isFrozen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform.position = startingpos.position;
        OrigSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFrozen)
        {
            OrigSpeed = speed;
            speed = 0;
        } else
        {
            speed = OrigSpeed;
        }

        if (platform.position == midOne.position)
        {
            midpoint1Reached = true;
        } else if(platform.position == midTwo.position)
        {
            midpoint2Reached = true;
        }
        if (platform.position == destination.position)
        {
            platform.position = startingpos.position;
            midpoint1Reached = false;
            midpoint2Reached = false;
        }

        
            if (midpoint2Reached)
            {
                platform.position = Vector3.MoveTowards(platform.position, destination.position, speed * Time.deltaTime);
            }
            else if (midpoint1Reached)
            {
                platform.position = Vector3.MoveTowards(platform.position, midTwo.position, speed * Time.deltaTime);
            }
            else
            {
                platform.position = Vector3.MoveTowards(platform.position, midOne.position, speed * Time.deltaTime);
            }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
            other.transform.position = platform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    public void freeze()
    {
        isFrozen = true;
        waitToUnfreeze();
    }

    public void unfreeze()
    {
        isFrozen = false;
    }

    IEnumerator waitToUnfreeze()
    {
        yield return new WaitForSeconds(10);
        unfreeze();
    }

}
