
using UnityEngine;
using UnityEngine.InputSystem;

public class settingsManager : gamemanager
{

    public static settingsManager Instance;

    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject cameraMenu;
    public GameObject activeMenu;

    //public GameObject player;
    //public PlayerController playerScript;
    cameraController playerCam;


    void Awake()
    {
        Instance = this;
        //player = gamemanager.instance.player;
        //playerScript = gamemanager.instance.playerScript;
        //playerCam = player.GetComponentInChildren<cameraController>();
        activeMenu = settingsMenu;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void enableMenu()
    {
        activeMenu.SetActive(true);
    }

    public void disableMenu()
    {
        activeMenu.SetActive(false);
    }

    public void openCameraMenu()
    {
        activeMenu = cameraMenu;
        enableMenu();
    }

    public void setCameraSensitivity(int amount)
    {
        //playerCam.sens = amount;
        PlayerPrefs.SetInt("Camera Sensivity", amount);
        PlayerPrefs.Save();
    }


    public void toggleYInversion()
    {
       //playerCam.invertY = true;
    }


}


