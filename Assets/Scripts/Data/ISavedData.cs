using UnityEngine;
// Code Written By Nathaniel King <3
// With help of how from https://www.youtube.com/watch?v=aUi9aijvpgs&list=WL&index=241&t=1422s.

public interface ISavedData
{
    void loadData(gameData data);
    void saveData(ref gameData data);
}
