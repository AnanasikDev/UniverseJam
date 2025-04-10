using Enemies;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;

    public BehaviourSettings settings;
    public StateMachine stateMachine;

    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private new MeshRenderer renderer;

    [ShowInInspector] public StateEnum currentState { get { return stateMachine?.currentState.type ?? StateEnum.Idle; } }
    public Vector3 vec2player { get { return PlayerController.instance.transform.position - transform.position; } }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        TryGetComponent<HealthComp>(out health);
        rigidbody = GetComponent<Rigidbody>();

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
            { StateEnum.Wander, Color.black },
        };

        renderer.material.color = getclr[state];
    }

    public bool SetPosition(Vector3 newPosition)
    {
        float desiredDiff = (newPosition - rigidbody.position).magnitude;
        Vector3 prevPos = rigidbody.position;
        rigidbody.MovePosition(newPosition);
        transform.position = rigidbody.position;
        float diff = (rigidbody.position - prevPos).magnitude;

        return diff > desiredDiff * 0.35f;
    }

    public bool AddPosition(Vector3 diff)
    {
        return SetPosition(rigidbody.position + diff);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}