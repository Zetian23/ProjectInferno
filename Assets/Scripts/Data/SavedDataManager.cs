using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using UnityEditor;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
// Code Written By Nathaniel King <3
// With help of how from https://www.youtube.com/watch?v=aUi9aijvpgs&list=WL&index=241&t=1422s.

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
        data.currLevel = SceneManager.GetActiveScene().buildIndex;

        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].saveData(ref data);
        }

        handler.Save(data);
    }
}
