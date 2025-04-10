using System.Collections.Generic;

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
                { StateEnum.Chase, new ChaseState(self) },
                { StateEnum.Attack, new AttackState(self) }
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
                        new Transition(StateEnum.Chase, StateEnum.Attack, () => AttackState.totalStatesActive < self.settings.maxAttackingEnemies),
                        new Transition(StateEnum.Chase, StateEnum.Idle, () => (PlayerController.instance.transform.position - self.transform.position).magnitude > self.settings.maxChaseDistance)
                    }
                },
                { StateEnum.Attack, new List<Transition>()
                    {
                        new Transition(StateEnum.Attack, StateEnum.Chase, () => true),
                        new Transition(StateEnum.Attack, StateEnum.Idle, () => true)
                    }
                }
            };

            currentState = enum2state[StateEnum.Idle];
            currentState.OnEnter();
        }

        public void Update()
        {
            if (GetNextState(out StateEnum state))
            {
                currentState.OnExit();
                currentState = enum2state[state];
                currentState.OnEnter();
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
        }
    }
}