using System.Collections.Generic;
using UnityEngine;
// Code Written By Nathaniel King <3
// With help of how from https://www.youtube.com/watch?v=aUi9aijvpgs&list=WL&index=241&t=1422s.

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
    public int currPower;
    public int currWeapon;

    public gameData()
    {
        powers = new bool[5];
        weapons = new weaponStats[5];
        bossDefeated = new bool[7];
        levelDefeated = new bool[7];
        respawnPoints = new Vector3[8];
        for (int i = 0; i < 7; i++)
            respawnPoints[i] = Vector3.zero;
        currLevel = 1;
        playerLevel = 0;
        currPower = 0;
        currWeapon = 0;
    }
}
