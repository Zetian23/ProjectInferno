using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public int levelToLoad;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SavedDataManager.instance.saveGame();
            gamemanager.instance.currLevel = levelToLoad;
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
