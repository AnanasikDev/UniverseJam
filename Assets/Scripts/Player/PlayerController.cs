using Sirenix.OdinInspector;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerStamina))]
[RequireComponent(typeof(HealthComp))]
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
    [HideInInspector] public Vector3 lastPosition { get; private set; }
    [HideInInspector] public Vector3 defaultPosition { get; private set; }
    public Vector3 deltaPositionSinceStart { get { return transform.position - defaultPosition; } }
    public Vector3 deltaPosition { get { return transform.position - lastPosition; } }
    public Vector3 movementDirection;

    [TitleGroup("Cursor")]
    /// <summary>
    /// Direction of cursor relative to the player
    /// </summary>
    [ReadOnly] public Vector3 cursorDirection = new Vector3(1, 0, 0);
    [ReadOnly] public Vector2 cursorDirection2D = new Vector2(1, 0);

    private new Rigidbody rigidbody;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public PlayerCamera playerCamera;
    [HideInInspector] public PlayerDash playerDash;
    [HideInInspector] public HealthComp healthComp;
    [HideInInspector] public PlayerStamina playerStamina;

    public event Action<Vector2> onMovingEvent;
    public event Action onStoppedEvent;

    public static PlayerController instance { get; private set; }

    private void Awake()
    {
        instance = this;

        rigidbody = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        playerCamera = GetComponent<PlayerCamera>();
        playerDash = GetComponent<PlayerDash>();
        healthComp = GetComponent<HealthComp>();
        playerStamina = GetComponent<PlayerStamina>();
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
        playerDash.Init();

        defaultPosition = transform.position;
        playerCamera.Init();

        playerStamina.Init();

        SetMovementInputFunction();
    }

    private void Update()
    {
        bool wasMoving = IsMoving;
        inputHorizontal = GetMovementInput("Horizontal");
        inputVertical = GetMovementInput("Vertical");
        if (wasMoving && !IsMoving)
        {
            onStoppedEvent?.Invoke();
        }
        cursorDirection2D = Input.mousePosition - playerCamera.camera.WorldToScreenPoint(transform.position);
        cursorDirection = new Vector3(cursorDirection2D.x, 0, cursorDirection2D.y);

        playerAttack.UpdateAttack();
        playerCamera.UpdateCamera();
    }

    private void FixedUpdate()
    {
        playerDash.UpdateDashing();
        if (!playerDash.blockMovementWhenDashing || !playerDash.isDashing)
            UpdateMotion();
    }

    private void UpdateMotion()
    {
        if (IsMoving)
        {
            //rigidbody.MovePosition(transform.position + new Vector3(inputHorizontal * speedX, 0, inputVertical * speedZ) * Time.fixedDeltaTime);
            rigidbody.position = rigidbody.position + new Vector3(inputHorizontal * speedX, 0, inputVertical * speedZ) * Time.fixedDeltaTime;
            transform.position = rigidbody.position;
            Vector3 diff = (transform.position - lastPosition);
            movementDirection = diff.normalized;
            onMovingEvent?.Invoke(diff);
            lastPosition = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, cursorDirection);
    }
}
