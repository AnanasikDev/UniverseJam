using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName="BehSettings_", menuName="Enemy Behaviour")]
    public class BehaviourSettings : ScriptableObject
    {
        [TitleGroup("Agent")]
        [Range(0, 12)] public float avoidanceRadius = 1.5f;
        [Range(0, 12)] public float weight = 1.5f;

        [TitleGroup("States")]
        public bool useFlee = true;
        public bool useStealth = true;
        public bool useWander = true;
        public bool useDash = false;

        [TitleGroup("Animations")]
        public float walkingAnimationSpeed;
        public float attackAnimationSpeed;
        public float attackExitTime = 1.2f;
        public float deathAnimationSpeed;

        [TitleGroup("Chase")]
        [Range(0, 40)] public float maxChaseDistance = 8;
        [MinMaxSlider(0, 8, showFields: true)] public Vector2 randomMovementSpeed = new Vector2(2.1f, 2.6f);

        [TitleGroup("Attack")]
        [Range(0, 9)] public float maxAttackDistance = 3;
        [Range(0, 9)] public float maxAttackApproachDistance = 2;
        [Range(0, 9)] public float minAttackApproachDistance = 1.5f;
        [Range(0, 100)] public float damagePerHit = 20;
        [Range(0, 8)]   public float hitIntervalSeconds = 1;

        [TitleGroup("Flee")]
        [ShowIf("useFlee")][Range(0, 25)] public float maxFleeDistance = 12;
        [ShowIf("useFlee")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomFleeSpeed = new Vector2(2f, 3f);
        [ShowIf("useFlee")][Range(0, 7)] public float minFleeTimeSeconds = 3f;
        [ShowIf("useFlee")][MinMaxSlider(0, 0.5f, showFields: true)] public Vector2 randomFleeChance = new Vector2(0.1f, 0.3f);

        [TitleGroup("Stealth")]
        [ShowIf("useStealth")][MinMaxSlider(0, 10, showFields:true)] public Vector2 randomStealthTargetDistance = new Vector2(3, 5.5f);
        [ShowIf("useStealth")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomStealthSpeed = new Vector2(1.4f, 2f);
        [ShowIf("useStealth")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomStealthDurationSeconds = new Vector2(1.2f, 2.2f);    
        [ShowIf("useStealth")][SuffixLabel("%")][Range(0, 100)] public float stealthChance = 30;

        [TitleGroup("Wander")]
        [ShowIf("useWander")][MinMaxSlider(0, 25, showFields:true)] public Vector2 randomWanderDistance = new Vector2(6, 12);
        [ShowIf("useWander")][MinMaxSlider(0, 5, showFields: true)] public Vector2 randomWanderSpeed = new Vector2(0.9f, 1.3f);
        [ShowIf("useWander")][SuffixLabel("%")][Range(0, 100)] public float wanderChance = 25;

        [TitleGroup("Dash")]
        [ShowIf("useDash")][Range(0, 6)] public float dashDistance = 4;
        [ShowIf("useDash")][MinMaxSlider(0, 1)] public Vector2 randomDashChance = new Vector2(0.1f, 0.2f);
        [ShowIf("useDash")][Range(0, 20)] public float dashSpeed = 9;
    }
}