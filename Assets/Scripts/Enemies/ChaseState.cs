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
            
        }
    }
}