using UnityEngine;

[CreateAssetMenu]

public class weaponStats : ScriptableObject
{
    public GameObject gunModel;
    public int shootDamage;
    public int shootDist;
    public float shootRate;
    public int ammoCur;
    public int ammoMax;
    public float reloadRate;

    public bool lazer;
    public bool spread;
    public bool headshots;

    public ParticleSystem shootEffect;
    public AudioClip shootSound;
    public float shootVol;
}
