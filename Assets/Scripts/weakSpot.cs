using System.Collections;
using UnityEngine;
// Written By Nathaniel
public class weakSpot : MonoBehaviour
{
    [SerializeField] Renderer model;

    [SerializeField] int hitAmount;
    [SerializeField] int damageMod;

    [SerializeField] float pulseDuration;
    [SerializeField] Color pulseColor;

    cagedEnemy parent;

    Color origColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origColor = model.material.color;
        parent = GetComponentInParent<cagedEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Pulse());
    }

    void takeDamage(int amount)
    {
        if (parent.HP > 0)
        {
            parent.HP -= amount * 2;
        }
        if(hitAmount <= 0 && parent.HP > 0)
        {
            Destroy(gameObject);
        }
        if (parent.HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(parent);
        }
    }

    IEnumerator Pulse()
    {
        float pulseTime = 0f;
        float currTime;
        while (pulseTime < pulseDuration)
        {
            pulseTime += Time.deltaTime;
            currTime = pulseTime / pulseDuration;
            yield return null;
            model.material.SetColor("_EmissionColor", Color.Lerp(origColor, pulseColor, currTime) * 50);
        }
        yield return new WaitForSeconds(.1f);
        pulseTime = 0f;
        currTime = 0f;
        while (pulseTime < pulseDuration)
        {
            pulseTime += Time.deltaTime;
            currTime = pulseTime / pulseDuration;
            yield return null;
            model.material.SetColor("_EmissionColor", Color.Lerp(pulseColor, origColor, currTime) * 50);
        }
        yield return new WaitForSeconds(1f);
    }
}
