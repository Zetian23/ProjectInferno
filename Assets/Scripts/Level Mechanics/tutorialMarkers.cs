using TMPro;
using UnityEngine;

public class TutorialBehavior : MonoBehaviour
{
    [SerializeField] tutorialText tuto;

    private void OnTriggerEnter(Collider other)
    {
        ITutorial tutorial = other.GetComponent<ITutorial>();


        if (tutorial != null )
        {

            tutorial.getTutorialInfo(tuto);

            gamemanager.instance.ShowTutorialMessage(tuto.message);
        }
    }

}
