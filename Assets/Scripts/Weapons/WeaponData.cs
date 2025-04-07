using UnityEngine;

[CreateAssetMenu(menuName="Weapon Data", fileName="Weapon_")]
public class WeaponData : ScriptableObject
{
    public new string name;
    [Range(0, 20)] public float damagePerUse = 20;
    [Range(0, 0.5f)] public float damageRandomization = 0.075f;
    [Range(0, 5)] public float reloadTimeSeconds = 0.6f;
    [Range(0, 5)] public float minDistance = 0;
    [Range(0, 12)] public float maxDistance = 12;
    [Range(0, 1)] public float critChange = 0.08f;
    [Range(0, 10)] public float critFactor = 3;
}