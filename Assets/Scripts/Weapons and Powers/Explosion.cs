using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] int damageAmount;
    [SerializeField] float speed;
    [SerializeField] float expansion;
    [SerializeField] Collider Collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < expansion)
        {
            transform.localScale += Vector3.one * speed * Time.deltaTime;
            Collider.transform.localScale += Vector3.one * speed * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damage = other.GetComponent<IDamage>();

        if (damage != null)
        {
            if (other.CompareTag("Player"))
            {
               damage.takeDamage(damageAmount / 4);
            }
            else
            {
                damage.takeDamage(damageAmount); ;
            }
        }

        IBurnable burn = other.GetComponent<IBurnable>();

        if (burn != null)
        {
            burn.melt();
        }
    }
}
