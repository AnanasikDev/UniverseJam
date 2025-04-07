using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class HealthComp : MonoBehaviour
{
    [ProgressBar("@MinHealth", "@MaxHealth", ColorGetter = "@Color.Lerp(Color.red, Color.green, Health / 100.0f)")]
    [ShowInInspector] public float Health { get; set; } = 100;
    [ShowInInspector] public float MinHealth { get; set; } = 0;
    [ShowInInspector] public float MaxHealth { get; set; } = 100;

    public HealthGroup group;

    /// <summary>
    /// Passed argument is the difference between new and old values.
    /// </summary>
    public event Action<float> onHealthChangedEvent;
    public event Action onDiedEvent;

    public virtual void SetHealth(float value)
    {
        Health = Mathf.Clamp(value, MinHealth, MaxHealth);
        if (value != Health)
        {
            onHealthChangedEvent?.Invoke(value - Health);
        }
        if (value <= MinHealth)
        {
            Die();
        }
    }

    public float GetHealth01()
    {
        return (Health - MinHealth) / (MaxHealth - MinHealth);
    }

    public virtual void Die()
    {
        onDiedEvent?.Invoke();
    }

    public enum HealthGroup
    {
        Enemy,
        Player
    }
}
