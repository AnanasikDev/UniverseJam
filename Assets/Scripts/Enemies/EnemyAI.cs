using Enemies;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;

    public BehaviourSettings settings;
    public StateMachine stateMachine;

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