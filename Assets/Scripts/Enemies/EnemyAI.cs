using Enemies;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;

    public BehaviourSettings settings;
    public StateMachine stateMachine;

    [SerializeField] private new MeshRenderer renderer;

    [ShowInInspector] public StateEnum currentState { get { return stateMachine?.currentState.type ?? StateEnum.Idle; } }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        TryGetComponent<HealthComp>(out health);

        stateMachine = new StateMachine();
        stateMachine.Init(this);
    }

    public void ChangeState(StateEnum state)
    {
        var getclr = new Dictionary<StateEnum, Color>()
        {
            { StateEnum.Idle, Color.green },
            { StateEnum.Chase, Color.blue },
            { StateEnum.Attack, Color.red },
            { StateEnum.Stealth, Color.magenta },
            { StateEnum.Flee, Color.yellow },
        };

        renderer.material.color = getclr[state];
    }

    private void Update()
    {
        stateMachine.Update();
    }
}