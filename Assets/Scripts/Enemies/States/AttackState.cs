using UnityEngine;

namespace Enemies
{
    public class AttackState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        private float lastHitTime;

        public AttackState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Attack;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > 1;
        }

        public override bool IsPossibleChangeTo()
        {
            return totalStatesActive < World.instance.globalEnemiesSettings.maxAttackingEnemies && self.vec2player.magnitude <= self.settings.maxAttackDistance + 0.2f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            if (Time.time - lastHitTime >= self.settings.hitIntervalSeconds)
            {
                lastHitTime = Time.time;
                Hit();
            }
        }

        private void Hit()
        {
            PlayerController.instance.healthComp.TakeDamage(self.settings.damagePerHit);
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxAttackDistance);
        }
    }
}