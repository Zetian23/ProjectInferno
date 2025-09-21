using UnityEngine;
using System.Collections;
// Code added by Nathaniel
// Code added by Naseem will be commented with "-N"
public class damage : MonoBehaviour
{
    enum damageType { moving, stationary, DOT, homing, death, jav, prideShot, laser }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject shockwave;

    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] bool laserShockwave;

    bool isGrounded;

    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isGrounded = false;
        if (type == damageType.moving || type == damageType.homing || type == damageType.laser)
        {
            Destroy(gameObject, destroyTime);

            if (type == damageType.moving || type == damageType.laser)
            {
                rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (type == damageType.homing)
        {
            rb.linearVelocity = (gamemanager.instance.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IDamage damage = other.GetComponent<IDamage>();

        if (damage != null && type != damageType.DOT)
        {
            damage.takeDamage(damageAmount);
        }

        //-N - Used for killplanes/instant kills
        if (damage != null && type == damageType.death) { 

            damage.takeDamage(777);
        }

        if (type == damageType.moving || type == damageType.homing || type == damageType.laser)
        {
            if(laserShockwave) Instantiate(shockwave, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }

        if (other.CompareTag("Platform"))
        {
            isGrounded = true;
        }

        if (other.CompareTag("Player") && type == damageType.jav)
        {
            rb.linearVelocity = Vector3.zero;
            isGrounded = true;
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        IDamage damage = other.GetComponent<IDamage>();

        if (damage != null && type == damageType.DOT)
        {
            if (!isDamaging)
            {
                StartCoroutine(damageOther(damage));
            }
        }
    }
    IEnumerator damageOther(IDamage dmg)
    {
        isDamaging = true;
        dmg.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

    public bool GetIfGrounded()
    {
        return isGrounded;
    }

    public void SetIfGrounded(bool grounded)
    {
        isGrounded = grounded;
    }
}