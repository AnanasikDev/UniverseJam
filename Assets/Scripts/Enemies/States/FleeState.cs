using UnityEngine;

namespace Enemies
{
    public class FleeState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public FleeState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Flee;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > 3;
        }

        public override bool IsPossibleChangeTo()
        {
            return true;
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
            Vector3 vec = (PlayerController.instance.transform.position - self.transform.position);

            self.transform.position += vec.normalized * -1 * self.settings.fleeSpeed * Time.deltaTime;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxFleeDistance);
        }
    }
}