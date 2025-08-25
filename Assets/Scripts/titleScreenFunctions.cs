using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor;

public class titlescreenFunctions : MonoBehaviour
{

    public static titlescreenFunctions instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject startText;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;

    [SerializeField] GameObject levelSelectMenu;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {

            Destroy(this.gameObject);

        }
        else
            instance = this;
    }

    void Update()
    {
        if (Input.anyKeyDown && startText.activeSelf)
        {

            startText.SetActive(false);
            menuActive = mainMenu;
            mainMenu.SetActive(true);

        } else if (menuActive != mainMenu)
        {
            mainMenu.SetActive(false);
        }

        menuSelection();
    }


    void menuSelection()
    {
        if (settingsMenu.activeSelf)
            menuActive = settingsMenu;
        else if (settingsMenu.activeSelf)
            menuActive = levelSelectMenu;

    }


}
