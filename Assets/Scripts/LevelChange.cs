using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public int levelToLoad;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
