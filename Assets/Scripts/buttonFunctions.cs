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

    public void loadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
        gamemanager.instance.stateUnpause();
    }


    public void openSettingsMenu()
    {

        settingsManager.Instance.enableMenu();

    }


    public void back()
    {

        settingsManager.Instance.disableMenu();
    }

    public void changeSensitivty()
    {


    }



}