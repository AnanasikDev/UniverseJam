using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName="Global Enemies Settings", fileName="GlobalEnemiesSettings_")]
    public class GlobalEnemiesSettings : ScriptableObject
    {
        [Range(0, 5)] public float maxAttackingEnemies = 2;
        [Range(0, 5)] public float maxChasingEnemies = 3;
        [Range(0, 5)] public float maxStealthEnemies = 2;
        [Range(0, 5)] public float maxFleeingEnemies = 2;
    }
}