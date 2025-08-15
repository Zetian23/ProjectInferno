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
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && type == itemType.healing)
        {
            dmg.takeDamage(modifierAmt);
            Destroy(gameObject);
        }
    }


}
