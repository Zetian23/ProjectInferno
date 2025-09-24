using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> whichEnemies;
    [SerializeField] bool isTimed;
    [SerializeField] bool isTrigger;
    [SerializeField] float spawnTime;
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
        else if (isTrigger && triggered)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        for (int i = 0; i < amount; i++)
        {
            randomEnemy = Random.Range(0, whichEnemies.Count);
            Instantiate(whichEnemies[randomEnemy], transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggered = true;
    }
}
