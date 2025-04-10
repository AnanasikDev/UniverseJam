using System;

namespace Enemies
{
    public abstract class State
    {
        public StateEnum type;
        protected EnemyAI self;
        public State(EnemyAI self)
        {
            this.self = self;
        }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();

        public abstract bool CanChangeFrom();
        public abstract bool CanChangeTo();

        public virtual void DrawGizmos() { }
    }

    public enum StateEnum
    {
        Idle,
        Chase,
        Attack,
        Stealth
    }
}