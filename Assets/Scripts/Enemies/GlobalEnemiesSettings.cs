using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "Global Enemies Settings", fileName = "GlobalEnemiesSettings_")]
    public class GlobalEnemiesSettings : ScriptableObject
    {
        [Range(0, 10)] public int maxAttackingEnemies = 2;
        [Range(0, 10)] public int maxChasingEnemies = 3;
        [Range(0, 10)] public int maxStealthEnemies = 2;
        [Range(0, 10)] public int maxFleeingEnemies = 2;
        [Range(0, 10)] public int maxDashingEnemies = 1;
    }
}