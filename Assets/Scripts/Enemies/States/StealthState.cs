using UnityEngine;

namespace Enemies
{
    public class StealthState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        private float minDuration;
        private float angularSign;

        public StealthState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Stealth;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > minDuration;
        }

        public override bool IsPossibleChangeTo()
        {
            return (totalStatesActive < World.instance.globalEnemiesSettings.maxStealthEnemies || AttackState.totalStatesActive > 2) && 
                Random.Range(0.0f, 1.0f) < (self.settings.stealthChance / 100.0f) * Time.deltaTime && 
                self.vec2player.magnitude < self.settings.maxChaseDistance && self.settings.useStealth;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;

            Vector3 vec = self.vec2player;
            minDuration = Random.Range(self.settings.randomStealthDurationSeconds.x, self.settings.randomStealthDurationSeconds.y);
            angularSign = Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1;
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            Vector3 vec = self.vec2player;
            Vector3 dir = new Vector3(-vec.z, 0, vec.x) * angularSign;

            float approachSign = 0;
            if (vec.magnitude > self.values.stealthTargetDistance + 0.1f) approachSign = 1;
            if (vec.magnitude < self.values.stealthTargetDistance - 0.1f) approachSign = -1;

            Vector3 angularVel = dir * self.values.stealthSpeed / Mathf.Sqrt(vec.magnitude / 2.0f);
            Vector3 approachVel = vec.normalized * approachSign * self.values.movementSpeed;

            self.Move((angularVel + approachVel) * Time.deltaTime);
        }
    }
}