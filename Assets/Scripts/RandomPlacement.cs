using UnityEngine;

public class RandomPlacement : MonoBehaviour
{
    public GameObject obj;
    public Transform plane;
    public int num = 50;
    

    void Start()
    {
        for(int i = 0; i < num/5; i++) { 
        for (int j = 0; j < num; j++)
        {
            float randomX = Random.Range(plane.position.x - plane.localScale.x * 4, plane.position.x + plane.localScale.x * 4);
            float randomZ = Random.Range(plane.position.z - plane.localScale.z * 4, plane.position.z + plane.localScale.z * 4);
            Vector3 randomPosition = new Vector3(randomX, plane.position.y, randomZ);
            GameObject newObj = Instantiate(obj, randomPosition, Quaternion.identity);
            newObj.transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        }
    }
}
