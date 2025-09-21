
using System.Collections;
using UnityEngine;

public class turretEnemy : CommonEnemyScript
{
    [SerializeField] float detRange;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletLife;
    [SerializeField] float fireRate;
    [SerializeField] float fireCooldown;
    [SerializeField] Transform barrel;
    [SerializeField] float bulletSpeed;

    public bool isDown;

    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {

        {
            fireCooldown -= Time.deltaTime;

            if (canSeePlayer() && playerInTrigger)
            {
                RotateBarrel(gamemanager.instance.player.transform.position - transform.position);
            }
            if (fireCooldown <= 0)
            {
                Attack();
            }
        }
    }


    private void RotateBarrel(Vector3 targetDir)
    {
        if(FOV == 360)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            barrel.rotation = Quaternion.Lerp(barrel.rotation, lookRotation, faceTargetSpeed * Time.deltaTime);
        }

        Vector3 horizonDir = targetDir;
        horizonDir.y = 0;

        if (horizonDir != Vector3.zero)
        {

            float angle = Vector3.SignedAngle(transform.forward, horizonDir, Vector3.up);

            float clampAngle = Mathf.Clamp(angle, -FOV / 2f, FOV /2f);
            Quaternion horizonRot = Quaternion.AngleAxis(clampAngle, Vector3.up);

            Vector3 tiltDir = horizonRot * Vector3.forward;
            tiltDir.y = targetDir.y;

            Quaternion lookRotation = Quaternion.LookRotation(tiltDir, Vector3.up);
            barrel.rotation = Quaternion.Lerp(barrel.rotation, lookRotation, 5 * Time.deltaTime);
        }
    }
    public override void Attack()
    {
        if (!isDown)
        {
            attackTimer = 0;
            anim.SetTrigger("Shoot");
            if (bullet != null && attackPos != null)
            {
                GameObject shot = Instantiate(bullet, attackPos.position, attackPos.rotation);

                Rigidbody rb = shot.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = false;
                    Vector3 shootDirection = (gamemanager.instance.player.transform.position - attackPos.position).normalized;
                    rb.AddForce(shootDirection * bulletSpeed, ForceMode.VelocityChange);
                }
                Destroy(shot, bulletLife);
            }
            fireCooldown = 1 / fireRate;
        }
    }

    public override void takeDamage(int amount)
    {
        Debug.Log("Ow");
        if (HP > 0)
        {

            HP -= amount;
           // agent.SetDestination(gamemanager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
        if (HP <= 0)
        {
            if (gamemanager.instance.currBoss == 3) isDown = true;
            else
            {
                gamemanager.instance.updateGameGoal(0, 0, -1);
                Destroy(gameObject);
                CallGainEXP();
            }
        }
    }
}
