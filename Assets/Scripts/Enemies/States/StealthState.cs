using UnityEngine;

namespace Enemies
{
    public class StealthState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public StealthState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Stealth;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > self.settings.stealthDurationSeconds;
        }

        public override bool IsPossibleChangeTo()
        {
            return (totalStatesActive < self.settings.maxStealthEnemies || AttackState.totalStatesActive > 2) && 
                Random.Range(0.0f, 1.0f) < self.settings.stealthChance * Time.deltaTime && 
                self.vec2player.magnitude < self.settings.maxChaseDistance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;

            Vector3 vec = self.vec2player;
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            Vector3 vec = self.vec2player;
            Vector3 dir = new Vector3(-vec.z, 0, vec.x);

            float sign = 0;
            if (vec.magnitude > self.settings.stealthTargetDistance + 0.1f) sign = 1;
            if (vec.magnitude < self.settings.stealthTargetDistance - 0.1f) sign = -1;

            Vector3 angularVel = dir * self.settings.stealthSpeed / Mathf.Sqrt(vec.magnitude / 2.0f);
            Vector3 approachVel = vec.normalized * sign * self.settings.movementSpeed;

            self.transform.position += (angularVel + approachVel) * Time.deltaTime;
        }
    }
}