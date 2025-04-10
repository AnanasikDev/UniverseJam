using Enemies;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;

    public BehaviourSettings settings;
    public StateMachine stateMachine;

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

    private void Update()
    {
        stateMachine.Update();
    }
}