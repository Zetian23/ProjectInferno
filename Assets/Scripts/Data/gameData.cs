using System.Collections.Generic;

[System.Serializable]
public class gameData
{
    public List<bool> powers;
    public List<weaponStats> weapons;

    public gameData()
    {
        powers = new ();
        weapons = new ();
    }
}
