using Enemies;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [HideInInspector] public HealthComp health;

    public BehaviourSettings settings;
    public EntityBehaviourValues values;

    public StateMachine stateMachine;

    [SerializeField] private new Rigidbody rigidbody;

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

        values = new EntityBehaviourValues();
        values.Init(settings);

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

    private Vector3 AvoidOthers(Vector3 direction)
    {
        float factorByDistance(float distance)
        {
            return distance > settings.avoidanceRadius ? 0 : 1.0f / Mathf.Clamp(Mathf.Pow(distance * 3.0f, 5), 0.01f, 100);
        }

        Vector3 diff = Vector3.zero;
        foreach (var enemy in World.instance.enemies)
        {
            if (enemy == this) continue;

            Vector3 vec = (transform.position - enemy.transform.position);
            diff += vec.normalized * factorByDistance(vec.magnitude) * enemy.settings.weight;
        }

        return direction + diff;
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

    public bool Move(Vector3 diff, bool avoidOthers = true)
    {
        if (avoidOthers) diff = AvoidOthers(diff);
        return SetPosition(rigidbody.position + diff);
    }

    private void Update()
    {
        stateMachine.Update();
    }
}