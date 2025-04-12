using UnityEngine;

namespace Enemies
{
    public abstract class AbstractEnemyAnimator : MonoBehaviour
    {
        protected EnemyAI self;
        protected Animator animator;
        [SerializeField] protected Transform model;
        public EnemyAnimatorCallback animatorCallback;

        public abstract void Init();
    }
}