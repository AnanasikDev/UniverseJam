using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName="BehSettings_", menuName="Enemy Behaviour")]
    public class BehaviourSettings : ScriptableObject
    {
        [TitleGroup("Global")]
        [Range(0, 5)] public float maxAttackingEnemies = 2;

        [TitleGroup("Instance-specific")]
        [Range(0, 20)] public float maxChaseDistance = 5;
        [Range(0, 9)] public float maxAttackDistance = 2;
        [Range(0, 5)] public float movementSpeed = 2;

        [Range(0, 100)] public float damagePerHit = 20;
        [Range(0, 5)]   public float hitIntervalSeconds = 1;
    }
}