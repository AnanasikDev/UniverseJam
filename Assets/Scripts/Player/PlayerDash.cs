using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private KeyCode dashKey = KeyCode.Space;
    [SerializeField][Range(0, 4)] private float dashDelaySeconds = 1.0f;
    [SerializeField][Range(0.4f, 40)] private float dashSpeed = 4.0f;
    [SerializeField][Range(0, 7)] private float dashDistance = 3.0f;
    [SerializeField] private AnimationCurve dashSpeedCurve;
    [SerializeField][Range(0, 90)] private float maxSlidingAngle = 80;
    public bool blockMovementWhenDashing = true;
    private float currentSpeed { get { return dashSpeedCurve.Evaluate(dashProgress01) * dashSpeed; } }
    [ShowInInspector] private float dashTime { get { return dashDistance / dashSpeed; } }
    private float lastTimeDashed = -10.0f;
    [ReadOnly] public bool isDashing = false;
    private Vector3 dashStartPosition;
    private Vector3 dashTargetPosition;
    private Vector3 dashDirection;
    private float dashProgress01 { get { return (Time.time - lastTimeDashed) / dashTime; } }

    private new Rigidbody rigidbody;

    public event Action onDashedEvent;

    public void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(rigidbody);
    }

    private bool CanDash()
    {
        return Time.time - lastTimeDashed > dashTime + dashDelaySeconds && PlayerController.instance.playerStamina.isUsable;
    }

    private void Update()
    {
        if (CanDash())
        {
            bool wasDashing = isDashing;
            isDashing = Input.GetKeyDown(dashKey);
            if (!wasDashing && isDashing)
            {
                StartDashing();
            }
        }
        if (Time.time - lastTimeDashed > dashTime && isDashing)
        {
            StopDashing();
        }
    }

    private void StartDashing()
    {
        dashStartPosition = transform.position;
        dashTargetPosition = transform.position + PlayerController.instance.movementDirection * dashDistance;
        lastTimeDashed = Time.time;
        dashDirection = (dashTargetPosition - dashStartPosition).normalized;
        dashDirection.y = 0;
        PlayerController.instance.playerStamina.UseStamina(PlayerController.instance.playerStamina.dashStaminaCost);
        onDashedEvent?.Invoke();
    }

    private void StopDashing()
    {
        isDashing = false;
    }

    public void UpdateDashing()
    {
        if (!isDashing) return;

        Vector3 newPos = rigidbody.position + dashDirection * currentSpeed * Time.fixedDeltaTime;
        bool isObscured = Physics.SphereCast
        (
            rigidbody.position + Vector3.up * 0.5f - dashDirection * 0.1f, 
            0.5f, 
            dashDirection, 
            out RaycastHit hitInfo, 
            Mathf.Clamp(currentSpeed * Time.fixedDeltaTime, 0.1f, 10.0f), 
            1 << 0, 
            QueryTriggerInteraction.Ignore
        );


        if (!isObscured)
        {
            rigidbody.position = newPos;
            transform.position = rigidbody.position;
        }
        else
        {
            float dot = Vector3.Dot(dashDirection, -hitInfo.normal);
            
            float angleToSurfaceRad = Mathf.PI / 2.0f - Mathf.Acos(dot);
            float angleToSurfaceDeg = angleToSurfaceRad * Mathf.Rad2Deg;

            Vector3 orthogonal = new Vector3(-hitInfo.normal.z, 0, hitInfo.normal.x);
            float orthogonalDirectionSign = Mathf.Sign(Vector3.Cross(dashDirection, hitInfo.normal).y);

            if (angleToSurfaceDeg > maxSlidingAngle)
            {
                StopDashing();
                return;
            }

            Vector3 prevTargetPos = dashTargetPosition;
            dashTargetPosition = rigidbody.position + (4) * orthogonal * orthogonalDirectionSign;
            dashDirection = (dashTargetPosition - rigidbody.position).normalized;
            dashDirection.y = 0;
        }
    }
}