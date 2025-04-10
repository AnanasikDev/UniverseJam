using UnityEngine;

namespace Enemies
{
    public class AttackState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public AttackState(EnemyAI self) : base(self)
        {

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
            
        }
    }
}