using System.Collections.Generic;
using System.Numerics;

[System.Serializable]
public class gameData
{
    public List<bool> powers;
    public List<weaponStats> weapons;
    public Vector3 respawnPoint;
    public List<bool> bossDefeated;
    public int Level;

    public gameData()
    {
        powers = new ();
        weapons = new ();
        bossDefeated = new List<bool>(7);
        respawnPoint = Vector3.Zero;
        Level = 0;
    }
}
