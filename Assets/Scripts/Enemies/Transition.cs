using System;

namespace Enemies
{
    public class Transition
    {
        public StateEnum from;
        public StateEnum to;
        public Func<State, bool> Condition;

        public Transition(StateEnum from, StateEnum to, Func<State, bool> specificCondition = null)
        {
            this.from = from;
            this.to = to;
            if (specificCondition == null)
            {
                specificCondition = (State state) => true;
            }
            Condition = specificCondition;
        }
    }
}