using UnityEngine;

namespace Enemies
{
    public class ChaseState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public Transform target;

        public ChaseState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Chase;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > 1;
        }

        public override bool IsPossibleChangeTo()
        {
            return self.vec2player.magnitude < self.settings.maxChaseDistance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;
            target = PlayerController.instance.transform;
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            Vector3 vec = (target.transform.position - self.transform.position);

            float sign = vec.magnitude > self.settings.minAttackApproachDistance ? 1 : -1;

            self.Move(vec.normalized * sign * self.values.movementSpeed * Time.deltaTime);
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxChaseDistance);
        }
    }
}