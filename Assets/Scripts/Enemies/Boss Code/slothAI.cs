using UnityEngine;
// Code Written By Nathaniel King <3
// COMPLETED

// Phase 1: (HP >= 80)
    // Melee Attack                 | Close range javelin attack.
// Phase 2: (HP >= 40)
    // Speed Boost & Melee Attack   | After hit to weak spot move quicker after a period of time, and close range javelin attack.
// Phase 3: (HP < 40)
    // Ranged Attack                | Javelin throw.

public class slothAI : sinEnemy
{
    [SerializeField] GameObject Javelin;
    [SerializeField] Rigidbody javRB;
    [SerializeField] float javThrowingSpeed;
    [SerializeField] float javThrowingDistance;
    SphereCollider damageTrigger;
    bool javThrown;
    bool javPickUp;
    Vector3 javStartPOS;
    Quaternion javStartRot;
    Vector3 javDirection;

    [SerializeField] float speedBoostLength;
    [SerializeField] float speedBoostMod;
    float speedBoostTimer;

    void Start()
    {
        InitVar();

        speedBoostTimer = speedBoostLength;

        damageTrigger = Javelin.GetComponent<SphereCollider>();

        javThrown = false;
        javPickUp = false;
        javStartPOS = javRB.transform.localPosition;
        javStartRot = javRB.transform.localRotation;

        gamemanager.instance.SetBossText("Sloth");
        gamemanager.instance.boss = gamemanager.bossType.sloth;
        gamemanager.instance.currBoss = 0;
        updateBossUI();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        checkHealth(150, 100);

        if (!javThrown)
        {
            if (playerInTrigger && canSeePlayer()) { }
        }
        else
        {
            if (javThrown == false && Javelin.GetComponent<damage>().GetIfGrounded() == true)
            {
                javRB.linearVelocity = Vector3.zero;
            }
            navToJav();
        }

        if (weakSpotHit == true && gamemanager.instance.GetPhase() == 2) speedBoost();
    }

    void speedBoost()
    {
        if (speedBoostTimer == speedBoostLength) agent.speed *= speedBoostMod;

        speedBoostTimer -= Time.deltaTime;

        if (speedBoostTimer <= 0f)
        {
            weakSpotHit = false;
            agent.speed = startSpeed;
            speedBoostTimer = speedBoostLength;
        }
    }

    void throwJavelin()
    {
        stoppingDistOrig = javThrowingDistance;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, javThrowingDistance, ~ignoreLayer) && !javThrown)
        {
            damageTrigger.enabled = true;
            javDirection = gamemanager.instance.player.transform.position - Javelin.transform.position;
            Javelin.transform.parent = null;
            javRB.isKinematic = false;
            javRB.useGravity = true;
            javPickUp = false;
            agent.speed *= speedBoostMod;
            javRB.linearVelocity = (javDirection.normalized) * javThrowingSpeed;
            javThrown = true;
        }
    }

    void navToJav()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(javRB.position);
        if (javPickUp)
        {
            damageTrigger.enabled = false;
            javRB.useGravity = false;
            javRB.isKinematic = true;
            javThrown = false;
            javPickUp = false;
            Javelin.transform.parent = this.transform;
            javRB.transform.localPosition = javStartPOS;
            javRB.transform.localRotation = javStartRot;
            agent.stoppingDistance = stoppingDistOrig;
            Javelin.GetComponent<damage>().SetIfGrounded(false);
            attackTimer = 0;
            agent.speed = startSpeed;
        }
    }

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (collider.CompareTag("Javelin") && Javelin.GetComponent<damage>().GetIfGrounded() == true)
        {
            javPickUp = true;
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Javelin") && Javelin.GetComponent<damage>().GetIfGrounded() == true)
        {
            javPickUp = true;
        }
    }

    public override void Attack()
    {
        base.Attack();

        if (gamemanager.instance.GetPhase() <= 2)
        {
            meleeAttack();
        }
        if (gamemanager.instance.GetPhase() > 2)
        {
            throwJavelin();
        }
    }
}
