using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.Collections.Generic;
using Unity.VisualScripting;
using NUnit.Framework.Internal;
using JetBrains.Annotations;
using System.Collections;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] public GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuLoad;

    [SerializeField] GameObject TutorialBox;
    [SerializeField] public GameObject hubWarning;

    [SerializeField] TMP_Text tutorialText;
    [SerializeField] TMP_Text ammoCurrentText;
    [SerializeField] TMP_Text reloadMessage;
    //[SerializeField] TMP_Text meleeEnemyCountText;
    //[SerializeField] TMP_Text rangedEnemyCountText;
    [SerializeField] TMP_Text bossEnemyCountText;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] TMP_Text waveCooldownText;
    [SerializeField] TMP_Text bossNameText;
    [SerializeField] public Vector3 levelStartPos;
    [SerializeField] public List<GameObject> bosses;

    public Image playerHPBar;
    public Image playerEXPBar;
    public GameObject playerDamageFlash;
    public GameObject playerHealFlash;
    public GameObject playerLevelUPFlash;
    public GameObject bossUI;
    public List<GameObject> bossHealthUI;
    public GameObject WaveUI;
    public GameObject WaveCooldownUI;
    public GameObject RemainingEnemiesUI;
    public GameObject fireIcon;
    public GameObject lightningIcon;
    public GameObject iceIcon;
    public GameObject windIcon;
    public GameObject stoneIcon;

    public GameObject iceTint;


    public List<Image> bossHPBar;

    public GameObject player;
    public playerController playerScript;

    public GameObject currentIcon;
    public GameObject previousIcon;

    public int playerAmmoCur;
    public int playerAmmoMax;

    public bool isPaused;
    public bool hubNotAvailible;
    public int lustIIIArcana;
    public int enemies;
    public int currBoss;
    public int currLevel;
    float timeScaleOrig;
    float warningTimer;
    bool waveTextIsActive;

    float tutorialTimer;

    public enum bossType { sloth, wrath, gluttony, envy, lust, greed };
    public bossType boss;

    //int meleeEnemyCount;
    //int rangedEnemyCount;
    //int bossEnemyCount;
    int currBossPhase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //bossEnemyCount = 1;
        instance = this;
        timeScaleOrig = Time.timeScale;
        lustIIIArcana = 4;
        warningTimer = 0;


        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if(menuActive == menuPause)
            {
                stateUnpause();
            }
        }

        if (hubNotAvailible)
        {
            warningTimer += 0.005f;

            if(warningTimer >= 1)
            {
                hubWarning.SetActive(false);
                warningTimer = 0;
                hubNotAvailible = false;
            }
        }

        displayAmmoConut();

        if (tutorialTimer >= 0)
        {
            tutorialTimer -= Time.deltaTime;

            if (tutorialTimer <= 0)
            {
                TutorialBox.SetActive(false);
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        if (WaveUI.activeInHierarchy)
        {
            waveTextIsActive = true;
            WaveUI.SetActive(false);
        }
        else
        {
            waveTextIsActive = false;
        }
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;   
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(waveTextIsActive) WaveUI.SetActive(true);
        menuActive.SetActive(false);
        menuActive = null;
        hubNotAvailible = false;
        hubWarning.SetActive(false);
        warningTimer = 0;
    }

    public void updateGameGoal(int nummel, int numran, int numboss)
    {
        //meleeEnemyCount += nummel;
        //rangedEnemyCount += numran;
        enemies += nummel + numran + numboss;
        enemiesLeftText.text = enemies.ToString("F0");

        //if (bossEnemyCount > 1)
        //    bossEnemyCount--;

        //meleeEnemyCountText.text = meleeEnemyCount.ToString("F0");
        //rangedEnemyCountText.text = rangedEnemyCount.ToString("F0");
        //bossEnemyCountText.text = bossEnemyCount.ToString("F0");
    }

    public void SetBossText(string boss)
    {
        bossNameText.text = boss;
    }

    public void SetWaveText(string wave)
    {
        waveText.text = wave;
    }

    public void ShowTutorialMessage(string message)
    {
        tutorialText.text = message;
        TutorialBox.SetActive(true);
        tutorialTimer = 7f;

    }


    public void displayAmmoConut()
    {
        reloadMessage.enabled = false;

        ammoCurrentText.text = playerAmmoCur.ToString("F0") + " / " + playerAmmoMax.ToString("F0");

        if (playerAmmoCur == 0 && playerAmmoMax != 0)
        {
            reloadMessage.enabled = true;
            //flashReloadText();

        }
    }



    
    public void DisplayPowerIcon(int power)
    {
        if (currentIcon != null) { 
        
            currentIcon.SetActive(false);
            previousIcon = currentIcon;
        }



        switch (power)
        {
            case 0:

                currentIcon = fireIcon;
                currentIcon.SetActive(true);
                break;
            case 1:

                currentIcon = lightningIcon;
                currentIcon.SetActive(true);
                break;
            case 2:


                currentIcon = iceIcon;
                currentIcon.SetActive(true);
                break;
            case 3:

                currentIcon = windIcon;
                currentIcon.SetActive(true);
                break;
            case 4:

                currentIcon = stoneIcon;
                currentIcon.SetActive(true);
                break;
                
        }

    }

    public void stateIceShock (bool active)
    {
        iceTint.SetActive(active); 
    }

    public void SetPhase(int phase) { currBossPhase = phase; }
    public int GetPhase() { return currBossPhase; }

    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void openLoad()
    {
        statePause();
        menuActive.SetActive(false);
        menuActive = menuLoad;
        menuActive.SetActive(true);

    }

    //IEnumerator flashReloadText()
    //{
    //    reloadMessage.enabled = true;
    //    yield return new WaitForSeconds(0.3f);
    //    reloadMessage.enabled = false;
    //}
}

