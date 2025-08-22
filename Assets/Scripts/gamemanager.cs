using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] TMP_Text meleeEnemyCountText;
    [SerializeField] TMP_Text rangedEnemyCountText;
    [SerializeField] TMP_Text bossEnemyCountText;
    [SerializeField] TMP_Text sinBossNameText;

    [SerializeField] int Wave;

    public Image playerHPBar;
    public Image playerEXPBar;
    public GameObject playerDamageFlash;
    public GameObject playerHealFlash;
    public GameObject playerLevelUPFlash;

    public Image bossHPBar;

    public GameObject player;
    public playerController playerScript;

    public bool isPaused;
    float timeScaleOrig;

    
    int meleeEnemyCount;
    int rangedEnemyCount;
    int bossEnemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        bossEnemyCount = 1;
        instance = this;
        timeScaleOrig = Time.timeScale;

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
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int numBoss, int nummel, int numran)
    {
        meleeEnemyCount += nummel;
        rangedEnemyCount += numran;
        bossEnemyCount += numBoss;
        

        meleeEnemyCountText.text = meleeEnemyCount.ToString("F0");
        rangedEnemyCountText.text = rangedEnemyCount.ToString("F0");
        bossEnemyCountText.text = bossEnemyCount.ToString("F0");

        if (bossEnemyCount > 1)
            bossEnemyCount--;
        
        if (bossEnemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void SinnerType(string sin)
    {
        sinBossNameText.text = sin;
    }

    public void updateWave(int wa)
    {
        Wave = wa;
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}

