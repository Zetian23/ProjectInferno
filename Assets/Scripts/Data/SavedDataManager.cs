using UnityEngine;

public class SavedDataManager : MonoBehaviour
{
    private gameData data;

    public static SavedDataManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one SaveDataManager in the scene");
        }
        instance = this;
    }

    public void newGame()
    {
        this.data = new gameData();
    }

    public void loadGame() 
    {
        if(this.data == null)
        {
            newGame();
        }
    }

    public void saveGame()
    {

    }
}
