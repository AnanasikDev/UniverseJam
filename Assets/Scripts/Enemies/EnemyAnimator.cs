using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : AbstractEnemyAnimator
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
                animator.SetTrigger("Attack");
            };

            self.health.onDiedEvent += () =>
            {
                animator.SetTrigger("Dead");
                self.enabled = false;
                self.health.UIBleedingBarInstance.gameObject.SetActive(false);
                self.health.UIHealthBarInstance.gameObject.SetActive(false);
            };
            self.onMovingEvent += (Vector2 diff) =>
            {
                if (diff.x != 0)
                {
                    animator.SetBool("Moving", true);
                    if (Mathf.Abs(diff.x) > 0.001f)
                        Flip(Mathf.Sign(diff.x));
                }
                else
                {
                    animator.SetBool("Moving", false);
                }
            };

            self.onStoppedEvent += () =>
            {
                animator.SetBool("Moving", false);
            };
        }

        public override void Flip(float sign)
        {
            if (!CanFlip()) return;
            base.Flip(sign);
            model.localScale = new Vector3(Mathf.Abs(model.localScale.x) * sign, model.localScale.y, model.localScale.z);
        }
    }
}