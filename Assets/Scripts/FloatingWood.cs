using UnityEngine;

public class FloatingWood : MonoBehaviour
{
    
    [SerializeField] int speed;
    [SerializeField] Transform platform;
    [SerializeField] Transform startingpos;
    [SerializeField] Transform destination;
    [SerializeField] Transform midOne;
    [SerializeField] Transform midTwo;
    [SerializeField] GameObject wood;
    bool midpoint1Reached = false;
    bool midpoint2Reached = false;
    

    Vector3 startingPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(platform.position == midOne.position)
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
        } else if (midpoint1Reached)
        {
            platform.position = Vector3.MoveTowards(platform.position, midTwo.position, speed * Time.deltaTime);
        }
        else
        {
            platform.position = Vector3.MoveTowards(platform.position, midOne.position, speed * Time.deltaTime);
        }
        
    }
}
