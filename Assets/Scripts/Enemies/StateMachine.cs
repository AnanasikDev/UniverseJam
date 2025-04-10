using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class StateMachine
    {
        public State currentState;
        public StateEnum defaultStateType;
        public EnemyAI self;

        public Dictionary<StateEnum, State> enum2state;
        public Dictionary<StateEnum, List<Transition>> stateTree;

        public void Init(EnemyAI self)
        {
            this.self = self;
            enum2state = new Dictionary<StateEnum, State>()
            {
                { StateEnum.Idle,    new IdleState(self)    },
                { StateEnum.Chase,   new ChaseState(self)    },
                { StateEnum.Attack,  new AttackState(self)  },
                { StateEnum.Stealth, new StealthState(self) },
                { StateEnum.Flee,    new FleeState(self)    },
            };

            stateTree = new Dictionary<StateEnum, List<Transition>>()
            {
                { StateEnum.Idle, new List<Transition>() 
                    {   
                        new Transition(StateEnum.Idle, StateEnum.Chase) 
                    } 
                },
                { StateEnum.Chase, new List<Transition>()
                    {
                        new Transition(StateEnum.Chase, StateEnum.Attack),
                        new Transition(StateEnum.Chase, StateEnum.Idle),
                        new Transition(StateEnum.Chase, StateEnum.Stealth),
                    }
                },
                { StateEnum.Attack, new List<Transition>()
                    {
                        new Transition(StateEnum.Attack, StateEnum.Chase),
                        new Transition(StateEnum.Attack, StateEnum.Idle)
                    }
                },
                { StateEnum.Stealth, new List<Transition>()
                    {
                        new Transition(StateEnum.Stealth, StateEnum.Chase),
                        new Transition(StateEnum.Stealth, StateEnum.Attack)
                    }
                },
                { StateEnum.Flee, new List<Transition>()
                    {
                        //new Transition(StateEnum.Flee, StateEnum.Chase),
                        //new Transition(StateEnum.Flee, StateEnum.Idle),
                    }
                }
            };

            currentState = enum2state[StateEnum.Idle];
            currentState.OnEnter();
            OnStateChanged();
        }

        public void Update()
        {
            if (GetNextState(out StateEnum state))
            {
                currentState.OnExit();
                Debug.Log($"Transition from {currentState.type} to {state}");
                currentState = enum2state[state];
                currentState.OnEnter();
                OnStateChanged();
            }

            currentState.OnUpdate();
        }

        private bool GetNextState(out StateEnum state)
        {
            foreach (Transition transition in stateTree[currentState.type])
            {
                if (transition.Condition(currentState) && 
                    enum2state[transition.from].IsPossibleChangeFrom() && 
                    enum2state[transition.to].IsPossibleChangeTo())
                {
                    state = transition.to;
                    return true;
                }
            }

            state = StateEnum.Idle;
            return false;
        }

        private void OnStateChanged()
        {
            self.ChangeState(currentState.type);
        }
    }
}