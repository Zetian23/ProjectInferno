using System.Collections;
using UnityEngine;

public class weakSpot : MonoBehaviour
{
    [SerializeField] Renderer model;

    [SerializeField] int hitAmount;
    [SerializeField] int damageMod;

    [SerializeField] float pulseDuration;
    [SerializeField] Color pulseColor;

    Color origColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        origColor = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Pulse());
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
