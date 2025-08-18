using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel
public class cagedEnemy : Enemy
{
    [SerializeField] LayerMask ignoreLayer;
    bool isCriticle;
    enum sinType { sloth, wrath, gluttony, envy, lust, greed, pride }
    [SerializeField] sinType sinner;
    [SerializeField] int waves;
    [SerializeField] List<Renderer> skinObjects;

    Color emissionColorOrig;

    int BHPOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager.instance.updateGameGoal(1, 0, 0);
        colorOrg = model.material.color;
        emissionColorOrig = model.material.GetColor("_EmissionColor");
        attackTimer = 0;
        BHPOrig = HP;
        updateBossUI();
    }

    // Update is called once per frame
    void Update()
    {
        singet();

        Debug.DrawRay(transform.position, playerDirection * attackDistance, Color.red);

        attackTimer += Time.deltaTime;

        if (playerInTrigger && canSeePlayer())
        {

        }
    }

    public override void faceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInTrigger = false;
    }

    public override void Attack()
    {
        switch (sinner) {

            case sinType.sloth:
                meleeAttack();
                break;

            case sinType.wrath:
                meleeAttack();
                break;

        }

        //attackTimer = 0;
        //Instantiate(bullet, attackPos.position, transform.rotation);
    }

    public override void takeDamage(int amount)
    {
        if (HP > 0)
        {
            HP -= amount;
            StartCoroutine(flashDamage());
            updateBossUI();
        }
        if (HP <= 0)
        {
            Destroy(gameObject);
            gamemanager.instance.updateGameGoal(-1, 0, 0);
        }
    }

    public IEnumerator flashDamage()
    {
        Debug.Log("In flash");
        for (int i = 0; i < skinObjects.Count; i++)
            skinObjects[i].material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < skinObjects.Count; i++)
            skinObjects[i].material.SetColor("_EmissionColor", emissionColorOrig);
    }

    void meleeAttack()
    {
        attackTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(attackDamage);
            }
        }
    }

    public void updateBossUI()
    {
        gamemanager.instance.bossHPBar.fillAmount = (float)HP / BHPOrig;
    }

    public void singet()
    {
        switch (sinner)
        {
            case sinType.sloth:
                gamemanager.instance.SinnerType("Sloth");
                break;
            case sinType.wrath:
                gamemanager.instance.SinnerType("Wrath");
                break;
            case sinType.gluttony:
                gamemanager.instance.SinnerType("Gluttony");
                break;
            case sinType.envy:
                gamemanager.instance.SinnerType("Envy");
                break;
            case sinType.lust:
                gamemanager.instance.SinnerType("Lust");
                break;
            case sinType.greed:
                gamemanager.instance.SinnerType("Greed");
                break;
            case sinType.pride:
                gamemanager.instance.SinnerType("Pride");
                break;
            default:
                gamemanager.instance.SinnerType("Unknown");
                break;
        }
    }

}