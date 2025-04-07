using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

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
    /// <summary>
    /// Passed argument is the amount of damage taken.
    /// </summary>
    public event Action<float> onDamagedEvent;
    public event Action onDiedEvent;

    [TitleGroup("UI")]
    [DisableInPlayMode] public bool doShowBar = true;
    [DisableInPlayMode][ShowIf("doShowBar")][SerializeField] private Canvas canvas;
    [DisableInPlayMode][ShowIf("doShowBar")][SerializeField][AssetsOnly] private UIBar UIBarPrefab;
    private UIBar UIBarInstance;

    private void Awake()
    {
        if (doShowBar)
        {
            Assert.IsNotNull(canvas);
            Assert.IsNotNull(UIBarPrefab);
            UIBarInstance = Instantiate(UIBarPrefab, canvas.transform);
            UIBarInstance.Init();
            UpdateUI(0);
            onHealthChangedEvent += UpdateUI;
        }
    }

    private void UpdateUI(float diff)
    {
        UIBarInstance.SetValue01(GetHealth01());
    }

    public virtual void GetDamage(float value)
    {
        SetHealth(Health - value);
    }

    public virtual void SetHealth(float value)
    {
        float prevHealth = Health;
        Health = Mathf.Clamp(value, MinHealth, MaxHealth);
        float diff = Health - prevHealth;
        if (diff != 0)
        {
            onHealthChangedEvent?.Invoke(diff);
            if (value < 0) onDamagedEvent?.Invoke(diff);
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
