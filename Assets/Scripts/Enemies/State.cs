namespace Enemies
{
    public abstract class State
    {
        public StateEnum type;
        protected EnemyAI self;
        private float startTime = 0;
        public float activeTime { get { return UnityEngine.Time.time - startTime; } }

        public State(EnemyAI self)
        {
            this.self = self;
        }
        public virtual void OnEnter()
        {
            startTime = UnityEngine.Time.time;
        }
        public abstract void OnExit();
        public abstract void OnUpdate();
        public virtual void OnFixedUpdate() { }

        public abstract bool IsPossibleChangeFrom();
        public abstract bool IsPossibleChangeTo();

        public virtual void DrawGizmos() { }
    }

    public enum StateEnum
    {
        Idle,
        Chase,
        Attack,
        Stealth,
        Flee,
        Wander,
        Dash
    }
}