using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public int levelToLoad;
    [SerializeField] Transform levelStartLocation;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SavedDataManager.instance.getData().currLevel = levelToLoad;
            SavedDataManager.instance.getData().respawnPoints[gamemanager.instance.currLevel - 1] = levelStartLocation.position;
            gamemanager.instance.currLevel = levelToLoad;
            SavedDataManager.instance.saveGame();
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
