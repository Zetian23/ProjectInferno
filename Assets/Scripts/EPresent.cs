using UnityEngine;

public class EPresent : MonoBehaviour
{
    
    public GameObject App;
    public GameObject Dis;
    GameObject[] enemies;
    bool gone = false;

    private void Update()
    {
        
        if (App != null)
        {
            if (gone)
            {
                App.SetActive(true);
                Destroy(this);
            }
            else
            {
                App.SetActive(false);
            }
        }

        if (Dis != null)
        {
            if (gone)
            {
               Destroy(Dis);
               Destroy(this);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
       if(enemies.Length <= 0) { gone = true; }else { gone = false; }
    }

}
