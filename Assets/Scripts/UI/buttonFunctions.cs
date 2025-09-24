using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpause();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }

    public void newGame()
    {
        SceneManager.LoadScene(1);
        SavedDataManager.instance.newGame();
    }

    public void load()
    {
        SavedDataManager.instance.loadGame();
        SceneManager.LoadScene(gamemanager.instance.currLevel);
        gamemanager.instance.stateUnpause();
    }

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        gamemanager.instance.stateUnpause();
    }

    public void save()
    {
        SavedDataManager.instance.saveGame();
    }

    public void returnToTitle()
    {
        SceneManager.LoadScene("Title Screen");
    }
    public void openSettingsMenu()
    {

        settingsManager.Instance.enableMenu();

    }

    public void back()
    {

        settingsManager.Instance.disableMenu();
    }

    public void viewCredits()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string currScene = currentScene.name;

        creditsManager.prevScene = currScene;
        SceneManager.LoadScene("Credits");


    }


    public void changeSensitivty()
    {


    }
}