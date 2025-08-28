
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class settingsManager : gamemanager
{

    public static settingsManager Instance;

    [SerializeField] GameObject parentMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject cameraMenu;
    [SerializeField] GameObject cameraSettingsMenu;
    [SerializeField] List<GameObject> menuList = new List<GameObject>();
    public GameObject activeMenu;
    public GameObject prevMenu;

    //public GameObject player;
    //public PlayerController playerScript;
    cameraController playerCam;
    int menuListPos;


    void Awake()
    {
        Instance = this;
        //playerCam = gamemanager.instance.player.GetComponentInChildren<cameraController>();
        menuList.Add(parentMenu);
        prevMenu = parentMenu;
        activeMenu = settingsMenu;
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void enableMenu()
    {
        menuList.Add(activeMenu);
        menuListPos++;
        menuList[menuListPos - 1].SetActive(false);
        menuList[menuListPos].SetActive(true);
    }

    public void disableMenu()
    {
        menuList[menuListPos].SetActive(false);
        menuList.Remove(menuList[menuListPos]);
        menuListPos--;
        menuList[menuListPos].SetActive(true);

    }

    public void returnToPreviousMenu()
    {
        GameObject tempMenu = activeMenu;
        disableMenu();
        activeMenu = prevMenu;
        prevMenu = tempMenu;
        enableMenu();
    }

    public void openCameraMenu()
    {
        activeMenu = cameraMenu;
        enableMenu();
    }


    public void setCameraSensitivity(int amount)
    {
        //playerCam.sensModifier = amount;
    }


    public void toggleYInversion()
    {
        //playerCam.invertYToggle = true;
    }


}


