using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.Collections.Generic;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuLoad;

    //[SerializeField] TMP_Text meleeEnemyCountText;
    //[SerializeField] TMP_Text rangedEnemyCountText;
    [SerializeField] TMP_Text bossEnemyCountText;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] TMP_Text waveCooldownText;
    [SerializeField] TMP_Text bossNameText;

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

    public List<Image> bossHPBar;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    public int lustIIIArcana;
    public int enemies;
    public int currBoss;
    float timeScaleOrig;
    bool waveTextIsActive;

    public enum bossType { sloth, wrath, gluttony, envy, lust, greed, pride };
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
}

