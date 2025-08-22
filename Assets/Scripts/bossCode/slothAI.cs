using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
// Code Written By Nathaniel King <3

// Phase 1: (HP >= 80)
    // Melee Attack                 | Close range javelin attack.
// Phase 2: (HP >= 40)
    // Speed Boost & Melee Attack   | After hit to weak spot move quicker after a period of time, and close range javelin attack.
// Phase 3: (HP < 40)
// Ranged Attack                    | Javelin throw.
public class slothAI : sinEnemy
{
    [SerializeField] GameObject Javelin;    // This is needed to be able to set and unset parent object and grab script of this object.
    [SerializeField] Rigidbody javRB;       // I use this for any change or set of position of the Javelin.
    [SerializeField] float javThrowingSpeed;// Set this to how fast the javelin will be thrown.
    bool javThrown;         // Checks if the Javelin is has been thrown
    bool javPickUp;         // Checks if the Javelin is in the bosses pickup zone.
    Vector3 javStartPOS;    // Needed to know the original LOCAL position.
    Quaternion javStartRot; // Needed to know the original LOCAL rotation.

    [SerializeField] float speedBoostLength;    // This is how long the speedboost last after sloth boss takes critical damage.
    [SerializeField] float speedBoostMod;       // This is how fast the boss will move during the boost.
    float speedBoostTimer;                      // This is for a count down of the speedboost time.
    float startSpeed;                           // This is the initial speed the boss moves at.

    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        speedBoostTimer = speedBoostLength;     // Initializing the set timer for when speedBoost() is called.
        startSpeed = agent.speed;               // Initializing how fast the boss was initially set to.

        javThrown = false;                              // Initializing that the javelin has not been thrown yet.
        javStartPOS = javRB.transform.localPosition;    // Initializing the LOCAL starting position.
        javStartRot = javRB.transform.localRotation;    // Initializing the LOCAL starting rotation.

        gamemanager.instance.SetBossText("Sloth");              // Setting the boss nametag to "Sloth".
        gamemanager.instance.boss = gamemanager.bossType.sloth; // Setting the bossType to the Sloth Boss.
    }

    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(80, 40);    // Checks the phases between the health periods.

        if (!javThrown) // Checks if the Javelin is not thrown
        {
            if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
        }
        else navToJav();// If the javelin has been thrown then the enemy needs to head straight to that position to grab it before returning focus on the player again.

        if (weakSpotHit == true && phase == 2) speedBoost();    // During the second phase if the weak spot is hit the boss speeds up.
    }

    void meleeAttack()  // Base attack for when the boss is close up attacking.
    {
        attackTimer = 0;// Reset the timer so that the attack will happen again after a period of time.

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, attackDistance, ~ignoreLayer)) // Draws a ling with the attackDistance to see if the player is within the distance.
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>(); // Initializing the IDamage script.

            if (dmg != null)    // Checks if the thing collided took damage.
            {
                dmg.takeDamage(attackDamage);   // Make the player take damage.
            }
        }
    }

    void speedBoost()
    {
        if (speedBoostTimer == speedBoostLength) agent.speed *= speedBoostMod;  //

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
        stoppingDistOrig = 10f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDirection, out hit, 10, ~ignoreLayer))
        { 
            Javelin.transform.parent = null;
            javRB.isKinematic = false;
            javRB.useGravity = true;
            javThrown = true;
            if (Javelin.GetComponent<damage>().GetIfGrounded() == false)
                javRB.linearVelocity = attackPos.forward * javThrowingSpeed;
            else
            {
                javRB.linearVelocity = Vector3.zero;
                agent.speed *= speedBoostMod;
            }
        }
    }

    void navToJav()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(javRB.position);
        if (javPickUp)
        {
            javRB.isKinematic = true;
            javRB.useGravity = false;
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
        
        if(collider.CompareTag("Javelin")) javPickUp = true;
    }

    public override void Attack()
    {
        if (phase <= 2)
        {
            meleeAttack();
        }
        if (phase > 2)
        {
            throwJavelin(); // TODO : Start The Javelin Collision Here so the player isn't damaged by the javelin durning the bosses basic attack
        }
    }
}
