using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEditor;
using Unity.VisualScripting;

public class SavedDataManager : MonoBehaviour
{
    [SerializeField] string file;

    gameData data;
    List<ISavedData> dataList;
    FileDataHandler handler;

    public static SavedDataManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one SaveDataManager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        handler = new FileDataHandler(Application.persistentDataPath, file);
        dataList = FindSavedData();
        loadGame();
    }

    private List<ISavedData> FindSavedData()
    {
        IEnumerable<ISavedData> newData = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISavedData>();
        return new List<ISavedData>(newData);
    }

    public ref gameData getData()
    {
        return ref data;
    }

    public void newGame()
    {
        data = new gameData();

        handler.Save(data);
    }

    public void loadGame() 
    {
        data = handler.Load();
        
        if(data == null)
        {
            newGame();
        }

        for (int i = 0; i < dataList.Count; i++) 
        {
            dataList[i].loadData(data);
        }
    }

    public void saveGame()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].saveData(ref data);
        }

        handler.Save(data);
    }
}
