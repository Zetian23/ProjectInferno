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

    public GameObject meleeModel;
    public int meleeDamage;
    public int meleeDist;
    public float meleeRate;

    public ParticleSystem meleeEffect;
    public AudioClip meleeSound;
    public float meleeVol;
}
