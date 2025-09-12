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

    public ParticleSystem shootEffect;
    public AudioClip shootSound;
    public float shootVol;
}
