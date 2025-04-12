using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimatorCallback : MonoBehaviour
    {
        public event Action onAttackPerformedEvent;
        public void OnAttackPerformed()
        {
            onAttackPerformedEvent?.Invoke();
        }
    }
}