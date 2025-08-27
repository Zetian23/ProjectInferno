using System.Collections;
using UnityEngine;
// Written By Nathaniel

public class weakSpot : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;        // Set this to the model of this Object.
    [SerializeField] Color pulseColor;      // Set this to the color you want to pulse to.
    [SerializeField] int hitAmount;         // Set this to the amount of hits the weakSpot can take.
    [SerializeField] int damageMod;         // Set this to the health amount extra that this wealSpot will take from the boss.
    [SerializeField] float startIntensity;  // Set this to the brightness you want to start with.
    [SerializeField] float endIntensity;    // Set this to the brightness you want to end with.
    [SerializeField] float pulseDuration;   // Set this to how long the pulse should last for.

    sinEnemy parent;    // This gets the objects parent class.

    Color origColor;    // This the color of this material.

    void Start()
    {
        origColor = model.material.color;           // Initialize the original color to the Material assigned to this.
        parent = GetComponentInParent<sinEnemy>();  // Initialize the parent script of the enemy.
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Pulse());    // Continous plusing when active.
    }

    IEnumerator Pulse() // This will flash a color emission from initial color to the set pulse color.
    {
        float pulseTime = 0f;   // This is the timer that will be used to incerement over time.
        float t;                // This is a set float that gets the mathimatical time it is at.

        while (pulseTime < pulseDuration)   // While the timer is less than the duration of the pulsing.
        {
            pulseTime += Time.deltaTime;                                                                        // Incerement the timer.
            t = Mathf.PingPong(Time.time, pulseDuration) / pulseDuration;                                       // Calculate the time of the pulse with a wave like the Sin() method (Based on the mathimatical sin wave; I love that math and code intertwine).
            yield return null;                                                                                  // Continues after a frame.
            model.material.SetColor("_EmissionColor", (Color.Lerp(origColor, pulseColor, t)) * endIntensity);   // Set the emission color of the material to slowly change to the pulse emission color.
        }
        yield return null;  // Continues after a frame.
        pulseTime = 0f;     // Resets pulse timer back to zero.
        t = 0f;             // Resets t to zero as we are gonna use it again.
        while (pulseTime < pulseDuration)   // While the timer is less than the duration of the pulsing.
        {
            pulseTime += Time.deltaTime;                                                                        // Incerement the timer.
            t = Mathf.PingPong(Time.time, pulseDuration) / pulseDuration;                                       // Calculate the time of the pulse with pingpong().
            yield return null;                                                                                  // Continues after a frame.
            model.material.SetColor("_EmissionColor", (Color.Lerp(pulseColor, origColor, t)) * startIntensity); // Set the emission color of the material to slowly change back to the original emission color.
        }
    }

    public void takeDamage(int amount)  // Uses this method to deal damage the enemy within this script.
    {
        if (!parent.isInvensible)   // If the enemy is in an invensible state then it shouldn't take any damage.
        {
            if (parent.HP > 0)  // If the enemy this is attached to has HP
            {
                parent.HP -= amount * damageMod;                // Then subtrack the health times the amount that the weakSpot times when hit,
                parent.StartCoroutine(parent.flashDamage());    // make the enemy flash damage,
                parent.updateBossUI();                          // update the bossUI to show the health missing if needed,
                parent.weakSpotHit = true;                      // set that weakSpot has been hit
                hitAmount--;                                    // and have the amount of hits left decrease.
            }
            if (hitAmount <= 0 && parent.HP > 0)    // If the amount of hits this spot can take has hit the limit and the boss still has health.
            {
                Destroy(gameObject);    // Then destroy this weakSpot.
            }
            if (parent.HP <= 0) // If the boss health is deplinished.
            {
                Destroy(parent.gameObject);                     // Then destroy the boss object
                gamemanager.instance.updateGameGoal(-1, 0, 0);  // and update the game goal to decrease the boss amount.
            }
        }
    }

    public void slothSlow(float percent) { }    // Set so that IDamage works.
}
