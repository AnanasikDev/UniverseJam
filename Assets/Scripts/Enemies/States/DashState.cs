using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemies
{
    public class DashState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        private float currentSpeed { get { return dashSpeed * (0.5f - 0.5f * Mathf.Cos((dashProgress01 + 0.1f) * Mathf.PI)); } }
        [ShowInInspector] private float dashTime { get { return dashDistance / dashSpeed; } }
        private float lastTimeDashed = -10.0f;
        [ReadOnly] public bool isDashing = false;
        private Vector3 dashStartPosition;
        private Vector3 dashTargetPosition;
        private Vector3 dashDirection;
        private float dashProgress01 { get { return (Time.time - lastTimeDashed) / dashTime; } }

        private Rigidbody rigidbody;

        public event System.Action onDashedEvent;

        private float dashSpeed = 4.0f;
        private float dashDistance = 3.0f;
        private float maxSlidingAngle = 80;

        public DashState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Dash;
            rigidbody = self.GetComponent<Rigidbody>();
            dashSpeed = self.settings.dashSpeed;
            dashDistance = self.settings.dashDistance;
        }

        public override bool IsPossibleChangeFrom()
        {
            return !isDashing;
        }

        public override bool IsPossibleChangeTo()
        {
            return totalStatesActive < World.instance.globalEnemiesSettings.maxDashingEnemies && self.settings.useDash &&
                Random.Range(0.0f, 1.0f) < self.values.dashChance * Time.deltaTime && self.vec2player.magnitude > self.settings.dashDistance / 2f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;
            StartDashing();
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            if (Time.time - lastTimeDashed > dashTime && isDashing)
            {
                StopDashing();
            }
        }

        public override void OnFixedUpdate()
        {
            UpdateDashing();
        }

        private void StartDashing()
        {
            isDashing = true;
            dashStartPosition = self.transform.position;
            dashTargetPosition = self.transform.position + self.vec2player * dashDistance;
            lastTimeDashed = Time.time;
            dashDirection = (dashTargetPosition - dashStartPosition).normalized;
            dashDirection.y = 0;
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
                self.transform.position = rigidbody.position;
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

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxFleeDistance);
        }
    }
}