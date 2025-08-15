using System.Collections;
using UnityEngine;
// Written By Nathaniel

public class weakSpot : MonoBehaviour, IDamage
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

    public void takeDamage(int amount)
    {
        if (parent.HP > 0)
        {
            parent.HP -= amount * 2;
            StartCoroutine(Pulse());
            parent.StartCoroutine(parent.flashDamage());
            hitAmount--;
        }
        if (hitAmount <= 0 && parent.HP > 0)
        {
            Destroy(gameObject);
        }
        if (parent.HP <= 0)
        {
            Destroy(parent);
            gamemanager.instance.updateGameGoal(-1, 0, 0);
        }
    }

    IEnumerator Pulse()
    {
        float pulseTime = 0f;
        float t;
        while (pulseTime < pulseDuration)
        {
            pulseTime += Time.deltaTime;
            t = Mathf.PingPong(Time.time, pulseDuration) / pulseDuration;
            yield return null;
            model.material.SetColor("_EmissionColor", (Color.Lerp(origColor, pulseColor, t)) * 50);
        }
        yield return null;
        pulseTime = 0f;
        t = 0f;
        while (pulseTime < pulseDuration)
        {
            pulseTime += Time.deltaTime;
            t = Mathf.PingPong(Time.time, pulseDuration) / pulseDuration;
            yield return null;
            model.material.SetColor("_EmissionColor", (Color.Lerp(pulseColor, origColor, t)) * 50);
        }
    }
}
