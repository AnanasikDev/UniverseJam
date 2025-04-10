using UnityEngine;

namespace Enemies
{
    public struct EntityBehaviourValues
    {
        public float movementSpeed;
        public float fleeSpeed;
        public float stealthSpeed;
        public float stealthDurationSeconds;
        public float stealthTargetDistance;
        public float wanderSpeed;

        public void Init(BehaviourSettings settings)
        {
            movementSpeed = Random.Range(settings.randomMovementSpeed.x, settings.randomMovementSpeed.y);
            fleeSpeed = Random.Range(settings.randomFleeSpeed.x, settings.randomFleeSpeed.y);
            stealthSpeed = Random.Range(settings.randomStealthSpeed.x, settings.randomStealthSpeed.y);
            stealthDurationSeconds = Random.Range(settings.randomStealthDurationSeconds.x, settings.randomStealthDurationSeconds.y);
            wanderSpeed = Random.Range(settings.randomWanderSpeed.x, settings.randomWanderSpeed.y);
            stealthTargetDistance = Random.Range(settings.randomStealthTargetDistance.x, settings.randomStealthTargetDistance.y);
        }
    }
}