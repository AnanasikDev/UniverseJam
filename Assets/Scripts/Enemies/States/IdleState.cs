namespace Enemies
{
    public class IdleState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        public IdleState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Idle;
        }

        public override bool IsPossibleChangeFrom()
        {
            return true;
        }

        public override bool IsPossibleChangeTo()
        {
            return self.vec2player.magnitude > self.settings.maxChaseDistance;
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
        }
    }
}