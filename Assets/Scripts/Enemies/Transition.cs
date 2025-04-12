using System;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class Transition
    {
        public StateEnum from;
        public StateEnum to;
        public Func<State, bool> Condition;
        public float delay = 0;
        public bool finished = false;

        public event Action onFinishedEvent;

        public Transition(StateEnum from, StateEnum to, Func<State, bool> specificCondition = null, float delay = 0)
        {
            this.from = from;
            this.to = to;
            if (specificCondition == null)
            {
                specificCondition = (State state) => true;
            }
            Condition = specificCondition;
            this.delay = delay;
        }

        public bool Start()
        {
            if (delay == 0)
            {
                onFinishedEvent?.Invoke();
                return true;
            }

            IEnumerator wait()
            {
                finished = false;
                yield return new WaitForSeconds(delay);
                finished = true;
                onFinishedEvent?.Invoke();
            }
            World.instance.StartCoroutine(wait());
            return false;
        }
    }
}