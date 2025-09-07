using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code Written By Nathaniel King <3
// Completed

public class Waves : MonoBehaviour
{
    [SerializeField] List<GameObject> SpawnPoints;
    [SerializeField] GameObject skeletons;
    [SerializeField] GameObject demons;
    [SerializeField] GameObject Boss;
    [SerializeField] int waves;
    [SerializeField] float waveSpawnCooldownTime;
    [SerializeField] float waveCooldownTime;

    Quaternion enemyRotation;
    int waveSpawnPointsAmount;
    int waveEnemyAmount;
    int currWave;
    int currSpawned;
    int choosenPoint;
    float waveSpawnCooldownTimer;
    float waveCooldownTimer;
    bool allEnemiesDead;
    bool bossIsSpawned;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRotation = Quaternion.Euler(0, 0, 0);
        allEnemiesDead = true;
        bossIsSpawned = false;
        currWave = 0;
        currSpawned = 0;
        gamemanager.instance.SetWaveText((currWave + 1).ToString());
        gamemanager.instance.WaveUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        waveSpawnCooldownTimer += Time.deltaTime;

        if (currWave == 0 && currSpawned <= waveSpawnPointsAmount)
        {
            if (allEnemiesDead)
            {
                allEnemiesDead = false;
                waveSpawnPointsAmount = Random.Range(0, 5);
            }
            if (waveSpawnCooldownTimer >= waveSpawnCooldownTime)
            {
                gamemanager.instance.RemainingEnemiesUI.SetActive(true);
                gamemanager.instance.WaveUI.SetActive(false);
                waveEnemyAmount = Random.Range(1, 11);
                choosenPoint = Random.Range(1, SpawnPoints.Count);
                StartCoroutine(flashSpawnIcon(SpawnPoints[choosenPoint]));
                initateWave();
                waveSpawnCooldownTimer = 0;
                currSpawned++;
            }
        }
        else if (currWave == 1 && currSpawned <= waveSpawnPointsAmount)
        {
            if (allEnemiesDead)
            {
                allEnemiesDead = false;
                waveSpawnPointsAmount = Random.Range(2, 9);
                Debug.Log(waveSpawnPointsAmount);
            }
            if (waveSpawnCooldownTimer >= waveSpawnCooldownTime)
            {

                gamemanager.instance.RemainingEnemiesUI.SetActive(true);
                gamemanager.instance.WaveUI.SetActive(false);
                waveEnemyAmount = Random.Range(5, 16);
                Debug.Log(waveEnemyAmount);
                choosenPoint = Random.Range(1, SpawnPoints.Count);
                StartCoroutine(flashSpawnIcon(SpawnPoints[choosenPoint]));
                initateWave();
                waveSpawnCooldownTimer = 0;
                currSpawned++;
            }
        }
        else if (currWave == 2 && currSpawned <= waveSpawnPointsAmount)
        {
            if (allEnemiesDead)
            {
                allEnemiesDead = false;
                waveSpawnPointsAmount = Random.Range(6, SpawnPoints.Count);
                Debug.Log(waveSpawnPointsAmount);
            }
            if (waveSpawnCooldownTimer >= waveSpawnCooldownTime)
            {

                gamemanager.instance.RemainingEnemiesUI.SetActive(true);
                gamemanager.instance.WaveUI.SetActive(false);
                waveEnemyAmount = Random.Range(10, 20);
                Debug.Log(waveEnemyAmount);
                choosenPoint = Random.Range(1, SpawnPoints.Count);
                StartCoroutine(flashSpawnIcon(SpawnPoints[choosenPoint]));
                initateWave();
                waveSpawnCooldownTimer = 0;
                currSpawned++;
            }
        }
        else if ((gamemanager.instance.enemies == 0) && !allEnemiesDead && (!bossIsSpawned) && (currSpawned >= waveSpawnPointsAmount))
        {
            if(waves == currWave)
            {
                Instantiate(Boss, SpawnPoints[0].transform.position, enemyRotation);
                StartCoroutine(flashSpawnIcon(SpawnPoints[0]));
                bossIsSpawned = true;
            }
            gamemanager.instance.RemainingEnemiesUI.SetActive(false);
            if(!bossIsSpawned) gamemanager.instance.WaveCooldownUI.SetActive(true);
            int timeTillWave = (int) (waveCooldownTime - waveSpawnCooldownTimer);
            gamemanager.instance.SetWaveCooldownText(timeTillWave.ToString("F0"));
            waveCooldownTimer += Time.deltaTime;
            if (waveCooldownTimer >= waveCooldownTime)
            {
                currWave++;
                gamemanager.instance.SetWaveText((currWave + 1).ToString());
                gamemanager.instance.WaveUI.SetActive(true);
                gamemanager.instance.WaveCooldownUI.SetActive(false);
                allEnemiesDead = true;
                currSpawned = 0;
                waveCooldownTimer = 0;
                waveSpawnCooldownTimer = 0;
            }
        }
    }

    void initateWave()
    {
        for (int i = 0; i < waveEnemyAmount; i++)
        {
            int enemyChoosen = Random.Range(0, 2);
            if (enemyChoosen == 0) 
            {
                Debug.Log("Skeleton Spawned");
                Instantiate(skeletons, SpawnPoints[choosenPoint].transform.position, enemyRotation); 
            }
            if (enemyChoosen == 1) 
            {
                Debug.Log("Demon Spawned");
                Instantiate(demons, SpawnPoints[choosenPoint].transform.position, enemyRotation);
            }
        }
    }

    IEnumerator flashSpawnIcon(GameObject icon)
    {
        icon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        icon.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        icon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        icon.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        icon.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        icon.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}
