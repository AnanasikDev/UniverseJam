using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthComp : MonoBehaviour
{
    [ProgressBar("@MinHealth", "@MaxHealth", ColorGetter = "@Color.Lerp(Color.red, Color.green, AbsoluteHealth / 100.0f)")]
    [ShowInInspector] public float AbsoluteHealth { get; set; } = 100;
    [ShowInInspector] public float MinHealth { get; set; } = 0;
    [ShowInInspector] public float MaxHealth { get; set; } = 100;

    [Tooltip("Multiplier for the incoming damage")][SerializeField][Range(0, 10)] private float incomingDamageFactor = 1;

    public bool IsAlive { get { return AbsoluteHealth > MinHealth; } }

    public HealthGroup group;

    /// <summary>
    /// Passed argument is the difference between new and old values.
    /// </summary>
    public event Action<float> onHealthChangedEvent;
    /// <summary>
    /// Passed argument is the amount of damage taken.
    /// </summary>
    public event Action<float> onDamagedEvent;
    public event Action<float> onBleedingChangedEvent;
    public event Action onDiedEvent;
    public static event Action<HealthComp> onAnyDiedEvent;

    [TitleGroup("Bleeding")]
    [SerializeField] private bool applyBleedingEffect = true;
    [SerializeField][ShowIf("applyBleedingEffect")][ProgressBar(0.0f, 1.0f, r: 1.0f, g: 0.3f, b: 0.25f)][ReadOnly] private float bleedingProgress = 0.0f;
    [SerializeField][ShowIf("applyBleedingEffect")][Range(1, 5)] private float bleedingDamageFactor = 3.0f;
    
    [Tooltip("Multiplier for the speed of building up bleeding")][SerializeField][ShowIf("applyBleedingEffect")][Range(0, 10)] private float bleedingSpeedFactor = 1.0f;
    private float currentBleedingDamageFactor = 1.0f;
    [SerializeField][ShowIf("applyBleedingEffect")][Range(0, 0.5f)] private float bleedingRestoreSpeed = 0.1f;

    [TitleGroup("UI")]
    [DisableInPlayMode] public bool doShowHealthBar = true;
    [DisableInPlayMode][SerializeField][ShowIf("applyBleedingEffect")] private bool doShowBleedingBar = true;
    [DisableInPlayMode] public bool instantiateNewBar = true;

    [DisableInPlayMode][SerializeField][ShowIf("@doShowHealthBar || doShowBleedingBar")] private Canvas canvas;
    [DisableInPlayMode][SerializeField][ShowIf("@(doShowHealthBar || doShowBleedingBar) && instantiateNewBar")][AssetsOnly] private AbstractUIProgress UIBarPrefab;
    [DisableInPlayMode][SerializeField][ShowIf("@doShowHealthBar && !instantiateNewBar")][SceneObjectsOnly] AbstractUIProgress UIHealthBarInstance;

    private AbstractUIProgress UIBleedingBarInstance;

    [DisableInPlayMode][SerializeField][ShowIf("@applyBleedingEffect && doShowBleedingBar")] private float bleedingBarShift = -25f;
    [DisableInPlayMode][SerializeField][ShowIf("doShowHealthBar")] private float healthBarShift = 0f;

    private void Awake()
    {
        if (doShowHealthBar)
        {
            Assert.IsNotNull(canvas);
            if (instantiateNewBar)
            {
                Assert.IsNotNull(UIBarPrefab);
                UIHealthBarInstance = Instantiate(UIBarPrefab, canvas.transform);
                UIHealthBarInstance.GetComponent<RectTransform>().anchoredPosition += Vector2.up * healthBarShift;
            }
            UIHealthBarInstance.Init(GetHealth01());
            onHealthChangedEvent += UpdateHealthUI;
        }
        if (applyBleedingEffect && doShowBleedingBar)
        {
            Assert.IsNotNull(canvas);
            if (instantiateNewBar) Assert.IsNotNull(UIBarPrefab);
            UIBleedingBarInstance = Instantiate(UIBarPrefab, canvas.transform);
            UIBleedingBarInstance.GetComponent<RectTransform>().anchoredPosition += Vector2.up * bleedingBarShift;
            UIBleedingBarInstance.GetForegroundColor = (float value) => Color.Lerp(new Color(0.961f, 0.353f, 0.192f), new Color(0.729f, 0.153f, 0.118f), value);
            UIBleedingBarInstance.Init(bleedingProgress);
            onBleedingChangedEvent += UpdateBleedingUI;
        }
    }

    private void Update()
    {
        if (!IsAlive) return;

        if (applyBleedingEffect)
        {
            Unbleed(bleedingRestoreSpeed * Time.deltaTime);
        }
    }

    private void UpdateHealthUI(float diff)
    {
        UIHealthBarInstance.SetValue01(GetHealth01());
    }
    private void UpdateBleedingUI(float diff)
    {
        UIBleedingBarInstance.SetValue01(bleedingProgress);
    }

    public virtual void Bleed(float value, float powerFactor = 1.0f)
    {
        this.bleedingProgress = Mathf.Clamp01(bleedingProgress + value);
        if (bleedingProgress >= 1.0f)
        {
            currentBleedingDamageFactor = bleedingDamageFactor * powerFactor;
            bleedingProgress = 0.0f;
        }
        onBleedingChangedEvent?.Invoke(value);
    }
    public virtual void Unbleed(float restoreValue)
    {
        this.bleedingProgress = Mathf.Clamp01(bleedingProgress - restoreValue);
        onBleedingChangedEvent?.Invoke(restoreValue);
    }

    public virtual void TakeDamage(float value, float bleedingValue = 0.0f, float bleedingPowerFactor = 1.0f)
    {
        if (!IsAlive) return;

        bleedingValue *= bleedingSpeedFactor;
        Bleed(bleedingValue, bleedingPowerFactor);
        value *= currentBleedingDamageFactor;
        value *= incomingDamageFactor;
        currentBleedingDamageFactor = 1.0f;
        SetHealth(AbsoluteHealth - value);
    }

    public virtual void SetHealth(float value)
    {
        float prevHealth = AbsoluteHealth;
        AbsoluteHealth = Mathf.Clamp(value, MinHealth, MaxHealth);
        float diff = AbsoluteHealth - prevHealth;
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
        return (AbsoluteHealth - MinHealth) / (MaxHealth - MinHealth);
    }

    public virtual void Die()
    {
        onDiedEvent?.Invoke();
        onAnyDiedEvent?.Invoke(this);
    }

    public enum HealthGroup
    {
        Enemy,
        Player
    }
}
