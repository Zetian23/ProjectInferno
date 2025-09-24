using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameData
{
    public bool[] powers;
    public weaponStats[] weapons;
    public Vector3[] respawnPoints;
    public bool[] bossDefeated;
    public bool[] levelDefeated;
    public int currLevel;
    public int playerLevel;

    public gameData()
    {
        powers = new bool[5];
        weapons = new weaponStats[5];
        bossDefeated = new bool[7];
        levelDefeated = new bool[7];
        respawnPoints = new Vector3[7];
        for (int i = 0; i < 7; i++)
            respawnPoints[i] = Vector3.zero;
        currLevel = 1;
        playerLevel = 0;
    }
}
