using UnityEngine;
using System.Collections.Generic;

public class healingSpread : MonoBehaviour
{
    [SerializeField] List<GameObject> healingLocations;
    [SerializeField] float spawnTime;
    float spawnTimer;
    List<bool> healingInPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = 0;
        healingInPos = new List<bool>() { false, false, false, false, false };
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnTime) 
        {
            int randomLocation = Random.Range(0, healingLocations.Count);
            for(int i = 0; i < healingLocations.Count; i++)
            {
                if (i == randomLocation && !healingInPos[i])
                {
                    healingLocations[i].SetActive(true);
                    healingInPos[i] = true;
                    spawnTimer = 0;
                }
            }
        }

        for (int i = 0; i < healingLocations.Count; i++)
        {
            if (healingLocations[i].activeInHierarchy)
            {
                healingInPos[i] = false;
            }
        }
    }
}
