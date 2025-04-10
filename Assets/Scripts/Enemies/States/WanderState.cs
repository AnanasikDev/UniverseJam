using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class WanderState : State
    {
        /// <summary>
        /// The number of currently running state machines where this type of state is active.
        /// </summary>
        public static int totalStatesActive = 0;

        private Vector3 target;

        public WanderState(EnemyAI self) : base(self)
        {
            this.type = StateEnum.Wander;
        }

        public override bool IsPossibleChangeFrom()
        {
            return activeTime > 1;
        }

        public override bool IsPossibleChangeTo()
        {
            return self.vec2player.magnitude > self.settings.maxChaseDistance && self.settings.useWander;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            totalStatesActive++;

            float angle = Random.Range(0, Mathf.PI * 2);
            float radius = Random.Range(self.settings.randomWanderDistance.x, self.settings.randomWanderDistance.y);

            target = (self.transform.position + PlayerController.instance.transform.position) / 2.0f + 
                    new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        }

        public override void OnExit()
        {
            totalStatesActive--;
        }

        public override void OnUpdate()
        {
            Vector3 vec = (target - self.transform.position);

            if (!self.Move(vec.normalized * self.values.wanderSpeed * Time.deltaTime))
            {
                Debug.Log("Wandering failed");
            }
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(self.transform.position, self.settings.maxChaseDistance);
        }
    }
}