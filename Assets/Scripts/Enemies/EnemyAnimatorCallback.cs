using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimatorCallback : MonoBehaviour
    {
        public event Action onAttackPerformedEvent;
        public event Action onAttackExitEvent;

        public void OnAttackPerformed()
        {
            onAttackPerformedEvent?.Invoke();
        }

        public void OnAttackExit()
        {
            onAttackExitEvent?.Invoke();
        }
    }
}