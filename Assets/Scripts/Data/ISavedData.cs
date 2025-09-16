using UnityEngine;

public interface ISavedData
{
    void loadData(gameData data);
    void saveData(ref gameData data);
}
