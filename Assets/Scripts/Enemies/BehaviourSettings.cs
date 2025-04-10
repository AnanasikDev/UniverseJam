using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName="BehSettings_", menuName="Enemy Behaviour")]
    public class BehaviourSettings : ScriptableObject
    {
        [TitleGroup("Global")]
        [Range(0, 5)] public float maxAttackingEnemies = 2;
        [Range(0, 5)] public float maxChasingEnemies = 3;
        [Range(0, 5)] public float maxStealthEnemies = 2;
        [Range(0, 5)] public float maxFleeingEnemies = 2;

        [TitleGroup("Chase")]
        [Range(0, 20)] public float maxChaseDistance = 8;
        [Range(0, 5)] public float movementSpeed = 2.5f;

        [TitleGroup("Attack")]
        [Range(0, 9)] public float maxAttackDistance = 3;
        [Range(0, 9)] public float minAttackDistance = 1.5f;
        [Range(0, 100)] public float damagePerHit = 20;
        [Range(0, 5)]   public float hitIntervalSeconds = 1;

        [TitleGroup("Flee")]
        [Range(0, 25)] public float maxFleeDistance = 12;
        [Range(0, 5)] public float fleeSpeed = 3f;
        [Range(0, 5)] public float minFleeTimeSeconds = 3f;

        [TitleGroup("Stealth")]
        [Range(0, 10)] public float stealthTargetDistance = 5f;
        [Range(0, 5)] public float stealthSpeed = 2f;
        [Range(0, 5)] public float stealthDurationSeconds = 1.9f;
        [SuffixLabel("%")][Range(0, 100)] public float stealthChance = 30;
    }
}