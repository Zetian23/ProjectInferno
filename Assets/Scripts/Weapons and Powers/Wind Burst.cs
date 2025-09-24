
using UnityEngine;

public class WindBurst : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float expansion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < expansion)
        {
            transform.localScale += new Vector3(1,0,1) * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
