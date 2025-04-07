using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool showAllValues = false;

    [TitleGroup("Movement")]
    [SerializeField][OnValueChanged("SetMovementInputFunction")] private bool useSmoothInput = true;

    [SerializeField][Range(0, 7)] private float speedX = 5;
    [SerializeField][Range(0, 7)] private float speedZ = 5;
    [ShowInInspector] public bool IsIdle { get { return Mathf.Approximately(inputHorizontal, 0) && Mathf.Approximately(inputVertical, 0); } }
    [ShowInInspector] public bool IsMoving { get { return !IsIdle; } }


    [SerializeField][ReadOnly][ShowIf("showAllValues")] private float inputHorizontal;
    [SerializeField][ReadOnly][ShowIf("showAllValues")] private float inputVertical;

    private Func<string, float> GetMovementInput = null;

    private new Rigidbody rigidbody;

    public static PlayerController instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void SetMovementInputFunction()
    {
        if (useSmoothInput)
        {
            GetMovementInput = Input.GetAxis;
        }
        else
        {
            GetMovementInput = Input.GetAxisRaw;
        }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        SetMovementInputFunction();
    }

    private void Update()
    {
        UpdateInput();
    }
    private void FixedUpdate()
    {
        UpdateMotion();
    }

    private void UpdateInput()
    {
        inputHorizontal = GetMovementInput("Horizontal");
        inputVertical = GetMovementInput("Vertical");
    }

    private void UpdateMotion()
    {
        if (IsIdle) return;

        rigidbody.MovePosition(transform.position + new Vector3(inputHorizontal * speedX, 0, inputVertical * speedZ) * Time.fixedDeltaTime);
    }
}
