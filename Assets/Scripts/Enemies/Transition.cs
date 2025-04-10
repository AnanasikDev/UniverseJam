using System;

namespace Enemies
{
    public class Transition
    {
        public StateEnum from;
        public StateEnum to;
        public Func<bool> Condition;

        public Transition(StateEnum from, StateEnum to, Func<bool> extraCondition)
        {
            this.from = from;
            this.to = to;
            Condition = extraCondition;
        }
    }
}