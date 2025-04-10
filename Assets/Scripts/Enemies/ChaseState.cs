using UnityEngine;

namespace Enemies
{
    public class ChaseState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public ChaseState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Chase;
        }

        public override bool CanChangeFrom()
        {
            return true;
        }

        public override bool CanChangeTo()
        {
            return (PlayerController.instance.transform.position - self.transform.position).magnitude <= self.settings.maxChaseDistance;
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
            self.transform.position += (PlayerController.instance.transform.position - self.transform.position).normalized * self.settings.movementSpeed * Time.deltaTime;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxChaseDistance);
        }
    }
}