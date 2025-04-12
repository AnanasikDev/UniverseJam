using UnityEngine;

namespace Enemies
{
    public abstract class AbstractEnemyAnimator : MonoBehaviour
    {
        protected EnemyAI self;
        protected Animator animator;
        [SerializeField] protected Transform model;
        public EnemyAnimatorCallback animatorCallback;
        protected float lastTimeFlipped;
        [SerializeField] protected float minFlipDelay = 0.3f;

        public virtual bool CanFlip() => Time.time - lastTimeFlipped > minFlipDelay;
        public virtual void Flip(float sign)
        {
            lastTimeFlipped = Time.time;
        }

        public abstract void Init();
    }
}