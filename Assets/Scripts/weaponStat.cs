using UnityEngine;

[CreateAssetMenu]
public class weaponStat : ScriptableObject
{
    public GameObject meleeModel;
    public GameObject rangeModel;
    [Range(1, 10)] public int shoDam;
    [Range(5, 1000)] public int shoDis;
    [Range(0.1f, 3)] public float shoRat;
    [Range(1, 10)] public int hitDam;
    [Range(5, 1000)] public int hitDis;
    [Range(0.1f, 3)] public float hitRat;
    public int amoCur;
    [Range(5, 50)] public int amoMax;

}
