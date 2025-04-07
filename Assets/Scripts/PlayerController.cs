using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerAttack))]
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
    private Vector3 lastPosition;
    private Vector3 defaultPosition;
    public Vector3 deltaPositionSinceStart { get { return transform.position - defaultPosition; } }
    public Vector3 deltaPosition { get { return transform.position - lastPosition; } }


    [TitleGroup("Camera")]
    [SerializeField][Required] private new Camera camera;
    [SerializeField][OnValueChanged("SetCameraFollowingMode")] private CameraFollowingMode cameraFollowingMode = CameraFollowingMode.Following;
    [SerializeField][ShowIf("cameraFollowingMode", CameraFollowingMode.Following)] private Vector3 cameraFollowingSpeed = new Vector3(5.0f, 0.0f, 1.5f);
    private Func<Vector3> GetCameraPosition = null;
    private Vector3 cameraDefaultLocalPosition;

    private new Rigidbody rigidbody;
    private PlayerAttack playerAttack;
    public static PlayerController instance { get; private set; }

    private void Awake()
    {
        instance = this;
        cameraDefaultLocalPosition = camera.transform.localPosition;
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

    private void SetCameraFollowingMode()
    {
        if (cameraFollowingMode == CameraFollowingMode.Static)
        {
            camera.transform.SetParent(null);
            GetCameraPosition = () => defaultPosition + cameraDefaultLocalPosition;
        }
        
        if (cameraFollowingMode == CameraFollowingMode.Following)
        {
            camera.transform.SetParent(null);
            GetCameraPosition = () =>
            {
                Vector3 result = new Vector3(
                    Mathf.Lerp(camera.transform.position.x, transform.position.x + cameraDefaultLocalPosition.x, Time.deltaTime * cameraFollowingSpeed.x),
                    Mathf.Lerp(camera.transform.position.y, transform.position.y + cameraDefaultLocalPosition.y, Time.deltaTime * cameraFollowingSpeed.y),
                    Mathf.Lerp(camera.transform.position.z, transform.position.z + cameraDefaultLocalPosition.z, Time.deltaTime * cameraFollowingSpeed.z)
                    );

                return result;
            };
        }

        if (cameraFollowingMode == CameraFollowingMode.Attached)
        {
            camera.transform.SetParent(transform);
            GetCameraPosition = () => transform.position + cameraDefaultLocalPosition;
        }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
        Assert.IsNotNull(camera);
        defaultPosition = transform.position;
        SetCameraFollowingMode();

        SetMovementInputFunction();
    }

    private void Update()
    {
        inputHorizontal = GetMovementInput("Horizontal");
        inputVertical = GetMovementInput("Vertical");
        playerAttack.UpdateAttack();
        camera.transform.position = GetCameraPosition();
    }

    private void FixedUpdate()
    {
        UpdateMotion();
    }

    private void UpdateMotion()
    {
        if (IsIdle) return;

        rigidbody.MovePosition(transform.position + new Vector3(inputHorizontal * speedX, 0, inputVertical * speedZ) * Time.fixedDeltaTime);
        lastPosition = transform.position;
    }

    public enum CameraFollowingMode
    {
        Static,
        Attached,
        Following
    }
}
