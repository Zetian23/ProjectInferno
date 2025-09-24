using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsManager : MonoBehaviour
{
    [SerializeField] float creditsSpeed;
    public static string prevScene;

    RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, creditsSpeed * Time.deltaTime);

        if (Input.anyKeyDown)
        {
            returnToPreviousScene();
        }
    }

    public void returnToPreviousScene()
    {
        SceneManager.LoadScene(prevScene);
    }
    
}
