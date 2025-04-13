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

        public bool isTransitioning = false;
        private bool isDead = false;
        public bool isLocked = false;

        public void Init(EnemyAI self)
        {
            this.self = self;
            enum2state = new Dictionary<StateEnum, State>()
            {
                { StateEnum.Idle,    new IdleState(self)    },
                { StateEnum.Chase,   new ChaseState(self)   },
                { StateEnum.Attack,  new AttackState(self)  },
                { StateEnum.Stealth, new StealthState(self) },
                { StateEnum.Flee,    new FleeState(self)    },
                { StateEnum.Wander,  new WanderState(self)  },
                { StateEnum.Dash,    new DashState(self)    },
            };

            stateTree = new Dictionary<StateEnum, List<Transition>>()
            {
                { StateEnum.Idle, new List<Transition>()
                    {
                        new Transition(StateEnum.Idle, StateEnum.Chase, (State state) => Room.currentRoom.index >= self.spawnRoom.index && (!self.settings.requirePlayerInTheRoom || Room.lastEnteredRoomIndex == Room.currentRoom.index)),
                        new Transition(StateEnum.Idle, StateEnum.Wander,
                            (State state) => self.vec2player.magnitude > self.settings.maxChaseDistance * 1.5f && Random.Range(0.0f, 1.0f) < (self.settings.wanderChance / 100.0f))
                    }
                },
                { StateEnum.Chase, new List<Transition>()
                    {
                        new Transition(StateEnum.Chase, StateEnum.Attack),
                        new Transition(StateEnum.Chase, StateEnum.Idle),
                        new Transition(StateEnum.Chase, StateEnum.Stealth),
                        new Transition(StateEnum.Chase, StateEnum.Flee),
                        new Transition(StateEnum.Chase, StateEnum.Dash),
                    }
                },
                { StateEnum.Attack, new List<Transition>()
                    {
                        new Transition(StateEnum.Attack, StateEnum.Chase, (State state) => self.vec2player.magnitude > self.settings.maxAttackDistance + 0.2f && self.animator.readyToSwitchState, delay: self.settings.attackExitTime),
                        new Transition(StateEnum.Attack, StateEnum.Idle, (State state) => self.animator.readyToSwitchState, delay: self.settings.attackExitTime)
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
                        new Transition(StateEnum.Flee, StateEnum.Chase),
                        new Transition(StateEnum.Flee, StateEnum.Idle)
                    }
                },
                { StateEnum.Wander, new List<Transition>()
                    {
                        new Transition(StateEnum.Wander, StateEnum.Idle, (State state) => state.activeTime > 5),
                        new Transition(StateEnum.Wander, StateEnum.Chase, (State state) => Room.currentRoom.index >= self.spawnRoom.index)
                    }
                },
                { StateEnum.Dash, new List<Transition>()
                    {
                        new Transition(StateEnum.Dash, StateEnum.Chase),
                        //new Transition(StateEnum.Dash, StateEnum.Attack)
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
                //Debug.Log($"Transition from {currentState.type} to {state}");
                currentState = enum2state[state];
                currentState.OnEnter();
                OnStateChanged();
            }

            currentState.OnUpdate();
        }

        public void FixedUpdate()
        {
            currentState.OnFixedUpdate();
        }

        private bool GetNextState(out StateEnum state)
        {

            state = StateEnum.Idle;
            if (isLocked) return false;
            if (isTransitioning) return false;

            foreach (Transition transition in stateTree[currentState.type])
            {
                if (transition.Condition(currentState) &&
                    enum2state[transition.from].IsPossibleChangeFrom() &&
                    enum2state[transition.to].IsPossibleChangeTo())
                {
                    if (transition.finished || transition.Start())
                    {
                        state = transition.to;
                        transition.finished = false;
                        return true;
                    }
                    else
                    {
                        isTransitioning = true;
                        transition.onFinishedEvent += () =>
                        {
                            isTransitioning = false;
                        };
                    }
                }
            }

            return false;
        }

        private void OnStateChanged()
        {
            self.ChangeState(currentState.type);
        }

        public bool SuggestTransition(StateEnum state)
        {
            foreach (Transition transition in stateTree[currentState.type])
            {
                if (transition.to == state && transition.Condition(currentState) &&
                    enum2state[transition.from].IsPossibleChangeFrom() &&
                    enum2state[transition.to].IsPossibleChangeTo())
                {
                    if (transition.finished || transition.Start())
                    {
                        state = transition.to;
                        transition.finished = false;
                        return true;
                    }
                    else
                    {
                        isTransitioning = true;
                        transition.onFinishedEvent += () =>
                        {
                            isTransitioning = false;
                        };
                        return true;
                    }
                }
            }
            return false;
        }

        public State ForceNewState(StateEnum state)
        {
            currentState.OnExit();
            currentState = enum2state[state];
            currentState.OnEnter();
            OnStateChanged();
            return currentState;
        }

        public void Die()
        {
            if (isDead) return;

            currentState.OnExit();
            isDead = true;
        }
    }
}