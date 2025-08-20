using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Code written by Nathaniel
public class sinEnemy : Enemy
{
    [SerializeField] LayerMask ignoreLayer;

    enum sinType        // This is initialized for the type of sin boss that is being used for 
    {                   // the gamemaker and what special moves are used for that boss.
        sloth,      // Attacks: Melee (Phase 1-2); Ranged (Phase 3)
            // Phase 1: (HP >= 80)
        // Melee Attack     | Close Range javelin attack.
            // Phase 2: (HP >= 40)
        // Speed Boost      | After hit to weak spot move quicker after a period of time.
            // Phase 3: (HP < 40)
        // Ranged Attack    | Javelin throw.

        // Special Skill Retrived: Slows enemies when attacked.
        wrath,      // Attacks: Melee (Phase 1-2); Ranged (Phase 3)
            // Phase 1: (HP >= 175)
        // Melee Attack     | Sword slash onto player.
            // Phase 2: (HP >= 100)
        // Spin Attack      | Spin with sword for a time and can be damage.
            // Phase 3: (HP < 100)
        // Eye Shot         | Eyes still attached will come off and move towards player dealing damage when collided.

        // Special Skill Retrived: Buff damage dealt by player.
        gluttony,   // Attacks: Melee (Phase 1-3)
            // Phase 1: (HP >= 300)
        // Slam Attack      | Slam club on groud causing a small splash damage.
            // Phase 2: (HP >= 200)
        // Spin Slam        | Spin for a bit then slam down with larger splash damage.
            // Phase 3: (HP < 200)
        // Jump Slam        | Jump's in the air after a cool down then has great splash damage.

        // Special Skill Retrived:
        envy,       // Attacks: Ranged (Phase 1-3)
            // Phase 1: (HP >= 600)
        // Range Attack     | Shoots a rappid lazor shot after attacking for a bit.
            // Phase 2: (HP >= 200)
        // Range Beam       | After a set of time there will be a Lazor beam that is shot out for a period of time and tracks player.
            // Phase 3: (HP < 200)
        // Electric Field   | Waves of electricity will shoot out for a period of time after a cooldown.

        // Special Skill Retrived: Stealing stats of an enemy after a period of time.
        lust,       // Attacks: Melee (Phase 1-3)
            // Phase 1: (HP >= 1400)
        // Jump Slam        | Jumps up then when hits the ground causes a splash damage.
            // Phase 2: (HP >= 1200)
        // Boss Split       | The boss splits into two different entities with 200 Healh and splits to two child enemies.
            // Phase 3: (HP >= 800)
        // Fill Poision     | Poision will fill slowly to a set level causing player to have to jump platform to platform.

        // Special Skill Retrived: Allows the player to regenerate health after a period of time.
        greed,

        // Special Skill Retrived: Extra XP is gain when collected.
        pride

        // Special Skill Retrived: Movement speed is increased.
    }

    [SerializeField] sinType sinner;
    [SerializeField] int waves;
    [SerializeField] List<Renderer> skinObjects;

    Color emissionColorOrig;

    int BHPOrig;
    float origIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager.instance.updateGameGoal(1, 0, 0);
        colorOrg = model.material.color;
        emissionColorOrig = skinObjects[0].material.GetColor("_EmissionColor");
        attackTimer = 0;
        BHPOrig = HP;
        updateBossUI();
    }

    // Update is called once per frame
    void Update()
    {
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
        gamemanager.instance.bossHPBar.fillAmount = (float) HP / BHPOrig;
    }

}