using UnityEngine;

namespace Enemies
{
    public class StealthState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        private Vector3 target;
        private float radius;

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
            return (StealthState.totalStatesActive < 3 || AttackState.totalStatesActive > 2) && Random.Range(0.0f, 1.0f) < 0.015f && (PlayerController.instance.transform.position - self.transform.position).magnitude < self.settings.maxChaseDistance;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;

            Vector3 vec = (PlayerController.instance.transform.position - self.transform.position);

            radius = vec.magnitude;
            target = self.transform.position + 2 * vec;
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            Vector3 vec = (PlayerController.instance.transform.position - self.transform.position);
            Vector3 dir = new Vector3(-vec.z, 0, vec.x);

            float sign = 0;
            if (vec.magnitude > self.settings.targetDistance + 0.1f) sign = 1;
            if (vec.magnitude < self.settings.targetDistance - 0.1f) sign = -1;

            Vector3 angularVel = dir * self.settings.stealthSpeed;
            Vector3 approachVel = vec.normalized * sign * self.settings.movementSpeed;

            self.transform.position += (angularVel + approachVel) * Time.deltaTime;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.66f);
            Gizmos.DrawWireSphere(target, 1);
        }
    }
}