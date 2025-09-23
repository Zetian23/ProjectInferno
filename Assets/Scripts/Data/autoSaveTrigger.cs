using UnityEngine;
using UnityEngine.SceneManagement;

public class autoSaveTrigger : MonoBehaviour
{
    [SerializeField] GameObject respawnPos;

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            SavedDataManager.instance.getData().respawnPoints[gamemanager.instance.currLevel] = respawnPos.transform.position;
            SavedDataManager.instance.saveGame();
        }
    }
}
