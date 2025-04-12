using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class ZombieAsPlayerAnimator : AbstractEnemyAnimator
    {
        public override void Init()
        {
            animator = GetComponentInChildren<Animator>();
            self = GetComponent<EnemyAI>();

            animator.SetFloat("MovementSpeedFac", self.settings.walkingAnimationSpeed);
            animator.SetFloat("AttackSpeedFac", self.settings.attackAnimationSpeed);
            animator.SetFloat("DeathSpeedFac", self.settings.deathAnimationSpeed);

            self.health.onDamagedEvent += (float value) => animator.SetTrigger("Hurt");
            ((AttackState)self.stateMachine.enum2state[StateEnum.Attack]).onStartedAttacking += () =>
            {
                if (Random.Range(0.0f, 1.0f) > 0.5f)
                    animator.SetTrigger("Attack1");
                else
                    animator.SetTrigger("Attack2");
            };
            self.onMovingEvent += (Vector2 diff) =>
            {
                if (diff.x != 0)
                {
                    animator.SetBool("IsRunning", true);
                    if (Mathf.Abs(diff.x) > 0.001f)
                        Flip(Mathf.Sign(diff.x));
                }
                else
                {
                    animator.SetBool("IsRunning", false);
                }
            };

            self.onStoppedEvent += () =>
            {
                animator.SetBool("IsRunning", false);
            };

            PlayerController.instance.onDiedEvent += OnPlayerDied;
        }

        private void Update()
        {

        }

        private void OnDisable()
        {
            if (PlayerController.instance) PlayerController.instance.onDiedEvent -= OnPlayerDied;
        }

        private void OnPlayerDied()
        {
            Room.currentRoom.Unlock(1);
            ChaseState state = (ChaseState)self.stateMachine.ForceNewState(StateEnum.Chase);
            self.stateMachine.isLocked = true;
            state.target = World.instance.rooms.Find(r => r.index == 1).gateCollider.transform;
        }

        public override void Flip(float sign)
        {
            if (!CanFlip()) return;
            base.Flip(sign);
            model.localScale = new Vector3(Mathf.Abs(model.localScale.x) * -sign, model.localScale.y, model.localScale.z);
        }
    }
}