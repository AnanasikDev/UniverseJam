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

        [TitleGroup("Agent")]
        [Range(0, 12)] public float avoidanceRadius = 1.5f;
        [Range(0, 12)] public float weight = 1.5f;

        [TitleGroup("States")]
        public bool useFlee = true;
        public bool useStealth = true;
        public bool useWander = true;

        [TitleGroup("Chase")]
        [Range(0, 20)] public float maxChaseDistance = 8;
        [MinMaxSlider(0, 5, showFields: true)] public Vector2 randomMovementSpeed = new Vector2(2.1f, 2.6f);

        [TitleGroup("Attack")]
        [Range(0, 9)] public float maxAttackDistance = 3;
        [Range(0, 9)] public float minAttackDistance = 1.5f;
        [Range(0, 100)] public float damagePerHit = 20;
        [Range(0, 5)]   public float hitIntervalSeconds = 1;

        [TitleGroup("Flee")]
        [ShowIf("useFlee")][Range(0, 25)] public float maxFleeDistance = 12;
        [ShowIf("useFlee")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomFleeSpeed = new Vector2(2f, 3f);
        [ShowIf("useFlee")][Range(0, 5)] public float minFleeTimeSeconds = 3f;

        [TitleGroup("Stealth")]
        [ShowIf("useStealth")][MinMaxSlider(0, 10, showFields:true)] public Vector2 randomStealthTargetDistance = new Vector2(3, 5.5f);
        [ShowIf("useStealth")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomStealthSpeed = new Vector2(1.4f, 2f);
        [ShowIf("useStealth")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomStealthDurationSeconds = new Vector2(1.2f, 2.2f);    
        [ShowIf("useStealth")][SuffixLabel("%")][Range(0, 100)] public float stealthChance = 30;

        [TitleGroup("Wander")]
        [ShowIf("useWander")][MinMaxSlider(0, 25, showFields:true)] public Vector2 randomWanderDistance = new Vector2(6, 12);
        [ShowIf("useWander")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomWanderSpeed = new Vector2(0.9f, 1.3f);
        [ShowIf("useWander")][SuffixLabel("%")][Range(0, 100)] public float wanderChance = 25;
    }
}