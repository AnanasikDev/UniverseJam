using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private EnemyAI self;
        private Animator animator;
        [SerializeField] private Transform model;
        public EnemyAnimatorCallback animatorCallback;

        public void Init()
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
                IEnumerator wait()
                {
                    yield return new WaitForSeconds(2);
                    Destroy(gameObject);
                }
                StartCoroutine(wait());
            };
            self.onMovingEvent += (Vector2 diff) =>
            {
                if (diff.x != 0)
                {
                    animator.SetBool("Moving", true);

                    if (Mathf.Abs(diff.x) > 0.01f) // to avoid jittering
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

        private void Flip(float sign)
        {
            model.localScale = new Vector3(Mathf.Abs(model.localScale.x) * sign, model.localScale.y, model.localScale.z);
        }
    }
}