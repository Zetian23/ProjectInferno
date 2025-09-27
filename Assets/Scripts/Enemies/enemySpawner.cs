using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
// Code Written By Nathaniel King <3

public class enemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> whichEnemies;
    [SerializeField] bool isTimed;
    [SerializeField] float spawnTime;
    [SerializeField] float ranged;
    [SerializeField] int amount;

    bool triggered;
    float spawnTimer;
    int randomEnemy;

    // Update is called once per frame
    void Update()
    {
        if (isTimed)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnTime)
            {
                Spawn();
                spawnTimer = 0;
            }
        }
        else if (triggered)
        {
            Spawn();
            Destroy(gameObject);
        }
    }

    void Spawn()
    {
        for (int i = 0; i < amount / 5; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                randomEnemy = Random.Range(0, whichEnemies.Count);
                float randomX = Random.Range(-ranged, ranged);
                float randomZ = Random.Range(-ranged, ranged);
                Vector3 randomPosition = new Vector3(transform.position.x - randomX, transform.position.y, transform.position.z - randomZ);

                NavMeshHit hit;

                if(NavMesh.SamplePosition(randomPosition, out hit, 100f, NavMesh.AllAreas))
                {
                    randomPosition.y = hit.position.y;
                    GameObject newObj = Instantiate(whichEnemies[randomEnemy], randomPosition, transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            triggered = true;
    }
}
