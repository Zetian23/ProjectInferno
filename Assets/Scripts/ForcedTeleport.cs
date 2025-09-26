using System.Collections;
using UnityEngine;

public class ForcedTeleport : MonoBehaviour
{
    [SerializeField] Transform teleportTarget;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SavedDataManager.instance.saveGame();
            SavedDataManager.instance.getData().respawnPoints[gamemanager.instance.currLevel - 1] = teleportTarget.position;
            other.transform.parent = transform;
            other.transform.position = teleportTarget.position;
            other.transform.parent = null;
        }
    }
}
