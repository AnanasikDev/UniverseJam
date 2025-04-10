using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField][ProgressBar(0, "maxStamina")] public float stamina;
    [SerializeField] public float maxStamina = 100;
    [ShowInInspector] public float stamina01 { get { return stamina / maxStamina; } }
    [Range(0, 100)] public float dashStaminaCost = 20;

    [SerializeField] private AnimationCurve regenSpeedOverValue;
    [Range(0.0f, 10.0f)] public float regenSpeedFactor = 1f;
    [Range(0.0f, 3.0f)] public float regenCooldownAfterUse = 0.8f;
    private float lastTimeUsed;

    [SerializeField] private AbstractUIProgress staminaUIBarInstance;
    public bool isUsable = true;

    public Func<float, Color> GetRegenFrom0ForegroundColor = (float value) => Color.Lerp(new Color(0.569f, 0.149f, 0.086f), new Color(0.77f, 0.36f, 0.48f), value);

    private Func<float, Color> defaultGetForegroundColor;



    public void Init()
    {
        Assert.IsNotNull(staminaUIBarInstance);
        defaultGetForegroundColor = staminaUIBarInstance.GetForegroundColor;
    }

    private void Update()
    {
        RegenStamina();
    }

    private void OnStaminaChanged()
    {
        staminaUIBarInstance.SetValue01(stamina01);
    }

    public void UseStamina(float diff)
    {
        lastTimeUsed = Time.time;
        SetStamina(stamina - diff);
    }

    public void SetStamina(float newValue)
    {
        float prevStamina = stamina;
        stamina = Mathf.Clamp(newValue, 0, maxStamina);
        float diff = stamina - prevStamina;
        if (diff != 0)
        {
            OnStaminaChanged();
        }

        if (newValue <= 0)
        {
            RegenStaminaFrom0();
        }
    }

    public void RegenStamina()
    {
        if (Time.time - lastTimeUsed < regenCooldownAfterUse)
        {
            return;
        }

        SetStamina(stamina + regenSpeedOverValue.Evaluate(stamina01) * regenSpeedFactor);
        if (stamina >= maxStamina && !isUsable)
        {
            isUsable = true;
            staminaUIBarInstance.GetForegroundColor = defaultGetForegroundColor;
        }
    }

    private void RegenStaminaFrom0()
    {
        isUsable = false;
        staminaUIBarInstance.GetForegroundColor = GetRegenFrom0ForegroundColor;
    }
}