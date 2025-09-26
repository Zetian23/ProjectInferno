using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//By Naseem
public class itemHandler : MonoBehaviour
{
    enum itemType { healing, buff, debuff }

    [SerializeField] itemType type;

    [SerializeField] int modifierAmt;

    //debug serialization fields
    //[SerializeField] bool isOneTimeUse;
    //[SerializeField] int despawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (type == itemType.healing)
            modifierAmt *= -1;

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();
            StartCoroutine(Healwait(dmg));
        }
    }
    IEnumerator Healwait(IDamage d)
    {
        yield return new WaitForSeconds(1.2f);
        

        if (d != null && type == itemType.healing)
        {
            d.takeDamage(modifierAmt);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
