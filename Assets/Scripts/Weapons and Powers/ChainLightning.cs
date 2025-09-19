using UnityEngine;

public class ChainLightning : MonoBehaviour
{

    [SerializeField] int damageAmount;
    [SerializeField] float speed;
    [SerializeField] float expansion;

    int chainCount;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chainCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < expansion)
        {
            transform.localScale += Vector3.one * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damage = other.GetComponent<IDamage>();

        if (damage != null && other.CompareTag("Enemy") && chainCount > 0)
        {
            if(other.transform.position != transform.position)
            {
                damage.takeDamage(damageAmount);
                transform.position = other.transform.position;
                transform.localScale = Vector3.one;
                chainCount--;
            }
        }
    }
}
