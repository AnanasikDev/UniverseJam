using System.Collections.Generic;
using System.Linq.Expressions;

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
                { StateEnum.Idle,  new IdleState(self)  },
                { StateEnum.Chase, new ChaseState(self) }
            };

            stateTree = new Dictionary<StateEnum, List<Transition>>()
            {
                { StateEnum.Idle, new List<Transition>() 
                    {   
                        new Transition(StateEnum.Idle, StateEnum.Chase, () => true) 
                    } 
                },
                { StateEnum.Chase, new List<Transition>()
                    {
                        new Transition(StateEnum.Chase, StateEnum.Attack, () => AttackState.totalStatesActive < self.settings.maxAttackingEnemies)
                    }
                },
                { StateEnum.Attack, new List<Transition>()
                    {
                        new Transition(StateEnum.Attack, StateEnum.Chase, () => true)
                    }
                },
                { StateEnum.Attack, new List<Transition>()
                    {
                        new Transition(StateEnum.Attack, StateEnum.Idle, () => true)
                    }
                }
            };

           /* stateTree = new Dictionary<StateEnum, List<StateEnum>>()
            {
                { StateEnum.Idle, new List<StateEnum>() { StateEnum.Chase } }
            };*/

            currentState = enum2state[StateEnum.Idle];
            currentState.OnEnter();
        }

        public void Update()
        {
            if (GetNextState(out StateEnum state))
            {
                currentState.OnExit();
                enum2state[state].OnEnter();
            }

            currentState.OnUpdate();
        }

        private bool GetNextState(out StateEnum state)
        {
            foreach (Transition transition in stateTree[currentState.type])
            {
                if (transition.Condition() && 
                    enum2state[transition.from].CanChangeFrom() && 
                    enum2state[transition.to].CanChangeTo())
                {
                    state = transition.to;
                    return true;
                }
            }

            state = StateEnum.Idle;
            return false;

            /*switch (currentState.type)
            {
                case StateEnum.Idle:
                    {

                    }
                    break;
                case StateEnum.Chase:
                    {

                    }
                    break;
                default:
                    {
                        
                    }
                    break;
            }*/
        }
    }
}