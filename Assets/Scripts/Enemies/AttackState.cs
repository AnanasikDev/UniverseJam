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

        public override bool CanChangeFrom()
        {
            return true;
        }

        public override bool CanChangeTo()
        {
            return (PlayerController.instance.transform.position - self.transform.position).magnitude <= self.settings.maxAttackDistance;
        }

        public override void OnEnter()
        {
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