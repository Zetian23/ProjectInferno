using UnityEngine;
using System.Collections.Generic;
// Code written by Nathaniel
public class cagedEnemy : Enemy
{
    bool isCriticle;
    enum sinType { sloth, wrath, gluttony, envy, lust, greed, pride }
    [SerializeField] sinType sinner;
    [SerializeField] int waves;
    [SerializeField] GameObject weakSpotObject;
    [SerializeField] List<Transform> weakSpotsPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gamemanager.instance.updateGameGoal(1);
        creatWeakSpots();
        colorOrg = model.material.color;
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (playerInTrigger)
        {
            playerDirection = gamemanager.instance.player.transform.position - transform.position;

            agent.SetDestination(gamemanager.instance.player.transform.position);

            if (attackTimer >= attackRate)
            {
                Attack();
            }
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
            }
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

    void creatWeakSpots()
    {
        for (int i = 0; i < weakSpotsPos.Count; i++)
            Instantiate(weakSpotObject, weakSpotsPos[i].position, transform.rotation, transform);
    }

    //public override void Attack()
    //{
    //    attackTimer = 0;
    //    Instantiate(bullet, attackPos.position, transform.rotation);
    //}

    public override void takeDamage(int amount)
    {
        if (HP > 0 && isCriticle)
        {
            HP -= amount * 2;
            StartCoroutine(flashRed());
        }
        if (HP > 0 && !isCriticle)
        {
            HP -= amount * 2;
            StartCoroutine(flashRed());
        }
        if (HP <= 0)
        {
            //gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
}