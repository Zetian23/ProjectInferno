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
    [SerializeField] GameObject Javelin;            // This is needed to be able to set and unset parent object and grab script of this object.
    [SerializeField] Rigidbody javRB;               // I use this for any change or set of position of the Javelin.
    [SerializeField] float javThrowingSpeed;        // Set this to how fast the javelin will be thrown.
    [SerializeField] float javThrowingDistance;     // Set this to how far you want the boss has to be to throw the weapon.
    SphereCollider damageTrigger;  // This will activate when the javelin is thrown and deal the thowing damage so when collided when not thrown wont deal damage.
    bool javThrown;         // Checks if the Javelin is has been thrown.
    bool javPickUp;         // Checks if the Javelin is in the bosses pickup zone.
    Vector3 javStartPOS;    // Needed to know the original LOCAL position.
    Quaternion javStartRot; // Needed to know the original LOCAL rotation.
    Vector3 javDirection;   // Needed to know where the player is to the javelin.

    [SerializeField] float speedBoostLength;    // This is how long the speedboost last after sloth boss takes critical damage.
    [SerializeField] float speedBoostMod;       // This is how fast the boss will move during the boost.
    float speedBoostTimer;                      // This is for a count down of the speedboost time.

    void Start()
    {
        InitVar(); // This calls the method in sinEnemy that initializes all fields in that script needed for this.

        speedBoostTimer = speedBoostLength;     // Initializing the set timer for when speedBoost() is called.
        startSpeed = agent.speed;               // Initializing how fast the boss was initially set to.

        damageTrigger = Javelin.GetComponent<SphereCollider>(); // Initializing the trigger to the javelin sphere collider.

        javThrown = false;                              // Initializing that the javelin has not been thrown yet.
        javPickUp = false;                              // Initializing that the javelin isn't picked up as it hasn't even been thrown.
        javStartPOS = javRB.transform.localPosition;    // Initializing the LOCAL starting position.
        javStartRot = javRB.transform.localRotation;    // Initializing the LOCAL starting rotation.

        gamemanager.instance.SetBossText("Sloth");              // Setting the boss nametag to "Sloth".
        gamemanager.instance.boss = gamemanager.bossType.sloth; // Setting the bossType to the Sloth Boss.
        updateBossUI();                                         // Initializing the boss UI.
    }

    void Update()
    {
        attackTimer += Time.deltaTime;  // Ticks the attackTimer up so it can know when to attack based off the attackRate.

        checkHealth(150, 100);    // Checks the phases between the health periods.

        if (!javThrown) // Checks if the Javelin is not thrown
        {
            if (playerInTrigger && canSeePlayer()) { }  // Checks if player is in the trigger and uses the canSeePlayer() method in the Enemy script.
        }
        else
        {
            if (javThrown == false && Javelin.GetComponent<damage>().GetIfGrounded() == true)   // When the javelin has hit the ground after its thrown.
            {
                javRB.linearVelocity = Vector3.zero;    // Setting the speed of the javelin to zero.
            }
            navToJav();// If the javelin has been thrown then the enemy needs to head straight to that position to grab it before returning focus on the player again.
        }

        if (weakSpotHit == true && phase == 2) speedBoost();    // During the second phase if the weak spot is hit the boss speeds up.
    }

    void speedBoost()   // Move the boss faster for a bit of time.
    {
        if (speedBoostTimer == speedBoostLength) agent.speed *= speedBoostMod;  // Speed up once the time has reset to the initial time.

        speedBoostTimer -= Time.deltaTime;  // Increases time down each second.

        if (speedBoostTimer <= 0f)  // Once the speedBoostTimer has hit zero.
        {
            weakSpotHit = false;                // Reset the statement for the speed boost to be activated again.
            agent.speed = startSpeed;           // Return the boss to the orginal speed when boost is over.
            speedBoostTimer = speedBoostLength; // Reset timer back to the length the boost will last.
        }
    }

    void throwJavelin() // Throws the javelin towards the player.
    {
        stoppingDistOrig = javThrowingDistance; // Increase the stopping distance to allow for further range.
        RaycastHit hit;                         // Initialize the raycast to find player.
        if (Physics.Raycast(transform.position, playerDirection, out hit, javThrowingDistance, ~ignoreLayer) && !javThrown) // If the player is in the way of the raycast.
        {
            damageTrigger.enabled = true;                                                               // Enable the collider so that it can hurt the player.
            javDirection = gamemanager.instance.player.transform.position - Javelin.transform.position; // Where to throw the javelin towards.
            Javelin.transform.parent = null;                                                            // Detach the javelin object from the boss.
            javRB.isKinematic = false;                                                                  // Set the object to be able to move.
            javRB.useGravity = true;                                                                    // Set the gravity to be able to act on the javelin.
            javPickUp = false;                                                                          // Set the javPickUp to false as the boss needs to go pick it up.
            agent.speed *= speedBoostMod;                                                               // Move the boss faster so it looks like it is sprinting.
            javRB.linearVelocity = (javDirection.normalized) * javThrowingSpeed;                        // Set the velocity of the javelin to move towards the player times the speed of the javelin.
            javThrown = true;                                                                           // Set javThrown to false as it has now been thrown.
        }
    }

    void navToJav() // Once the javelin has been thrown the boss will navigate to it.
    {
        agent.stoppingDistance = 0;             // Sets the stopping agent to 0 so that the boss will not stop before colliding with the javelin.
        agent.SetDestination(javRB.position);   // Sets the position the boss needs to go as the javelin's rigidbody.
        if (javPickUp)  // If the javelin has been picked up.
        {
            damageTrigger.enabled = false;                      // Disable the trigger so that the player can't hit it when not thrown.
            javRB.useGravity = false;                           // Disable gravity so when put back in place it doesn't fall.
            javRB.isKinematic = true;                           // Makes the object unablr to move from current LOCAL location.
            javThrown = false;                                  // Javelin is now picked up so it has not been thrown.
            javPickUp = false;                                  // Now that the code called for it, change it to false to initialize the next time the javelin is thrown.
            Javelin.transform.parent = this.transform;          // Attaching back to this object to allow it to follow the boss.
            javRB.transform.localPosition = javStartPOS;        // Placing it back in the exact place it was before being thrown on the boss.
            javRB.transform.localRotation = javStartRot;        // Rotating it back in the exact place it was before being thrown on the boss.
            agent.stoppingDistance = stoppingDistOrig;          // Return the stopping distance so the boss can attack from afar.
            Javelin.GetComponent<damage>().SetIfGrounded(false);// Set that the object is no longer grounded.
            attackTimer = 0;                                    // Restart the timer so the boss doesn't instantly throw javelin.
            agent.speed = startSpeed;                           // Slow the boss back down.
        }
    }

    protected override void OnTriggerEnter(Collider collider)   // Overrides the OnTriggerEnter so that we can have a trigger apart from the base method.
    {
        base.OnTriggerEnter(collider);  // Calls the OnTriggerEnter(collider) from the Enemy class.

        if (collider.CompareTag("Javelin") && (Javelin.GetComponent<damage>().GetIfGrounded() == true || javRB.linearVelocity == Vector3.zero))   // Checks if the object that collided is on the ground and is the javelin.
        {
            javPickUp = true;   // Sets the javPickUp to true so that in navToJav() it picks it up.
        }
    }

    void OnTriggerStay(Collider collider)   // This is called when an object stays in the collider like if it was thrown at the player nearby and never left the collider.
    {
        if (collider.CompareTag("Javelin") && (Javelin.GetComponent<damage>().GetIfGrounded() == true || javRB.linearVelocity == Vector3.zero))   // Checks if the collider is the javelin and is on the ground so it won't just keep picking it up.
        {
            javPickUp = true;   // Sets the javPickUp to true so that in navToJav() it picks it up.
        }
    }

    public override void Attack()   // Overrides the Attack() method to fit the Sloth Boss.
    {
        if (phase <= 2) // Checks if it's phase one or two.
        {
            meleeAttack();  // If so then the meleeAttack() is called for close up damage.
        }
        if (phase > 2)  // Checks if it has hit the third phase.
        {
            weakSpotHit = false;        // Set weakSpot to false.
            agent.speed = startSpeed;   // Set speed back to the startingSpeed.
            throwJavelin();             // Then the boss will throw the javelin.
        }
    }
}
