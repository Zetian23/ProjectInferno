using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] float AOERange;
    [SerializeField] float AOESpeed;
    [SerializeField] int damageAmount;
    [SerializeField] Collider Collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < AOERange)
        {
            transform.localScale += Vector3.one * AOESpeed * Time.deltaTime;
            Collider.transform.localScale += Vector3.one * AOESpeed * Time.deltaTime;
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
            damage.takeDamage(damageAmount);
        }

        IBurnable melt = other.GetComponent<IBurnable>();

        if (melt != null)
        {
            melt.Melt();
        }
    }

}
