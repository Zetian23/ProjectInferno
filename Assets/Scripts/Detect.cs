using System.Collections;
using UnityEngine;

public class Detect : MonoBehaviour
{
    public GameObject enemy;
    public GameObject App;
    public GameObject Dis;


    private void Update()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (App != null) { 
            if (enemy == null)
            {
                App.SetActive(true);
            }
            else
            {
              App.SetActive(false);
            }
        }

        if (Dis != null)
        {
            if (enemy == null)
            {
                Dis.SetActive(false);
            }
            else
            {
                Dis.SetActive(true);
            }
        }
    }
}
