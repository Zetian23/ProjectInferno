using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class skeletonReanimate : CommonEnemyScript
{

    [SerializeField] private int maxReanimations;
    [SerializeField] private int reanimateDelay;
    [SerializeField] private float reanimateAnimTime;
    [SerializeField] private int reanimateHP;

    private int reanimationCount = 0;
    private bool isDead = false;
    private bool isReanimating = false;

    public override void takeDamage(int amount)
    {
       if(HP <= 0 || isDead || isReanimating)
        {
            return;
        }

         HP -= amount;
        agent.SetDestination(gamemanager.instance.player.transform.position);
        StartCoroutine(flashDamage());

        if(HP <= 0)
        {
            if(reanimationCount < maxReanimations)
            {
                StartCoroutine(Reanimate());
            }
            else
            {
                anim.SetTrigger("Death");
                gamemanager.instance.updateGameGoal(0, 0, -1);
                Destroy(gameObject);
                CallGainEXP();
            }
        }
    }


    private IEnumerator Reanimate()
    {
        isDead = true;
        isReanimating = true;
        reanimationCount++;

        anim.SetTrigger("Death");
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(reanimateDelay);

        anim.SetTrigger("Reanimate");
       

        yield return new WaitForSeconds(reanimateAnimTime);
        HP = reanimateHP;
        agent.isStopped = false;
        GetComponent <Collider>().enabled = true;
        isDead = false;
        isReanimating = false;


    }

    public override void Attack()
    {
        if (isReanimating) return;
        base.Attack();
    }
}
