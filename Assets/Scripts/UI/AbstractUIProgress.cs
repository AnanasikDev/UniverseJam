using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractUIProgress : MonoBehaviour
{
    [SerializeField] protected Image background;
    [SerializeField] protected Image foreground;
    [ReadOnly][ShowInInspector] public float Value { get; protected set; } = 1.0f;
    [SerializeField][OnValueChanged("SetDoAnimateColor")] protected bool doAnimateForegroundColor = true;
    public Func<float, Color> GetForegroundColor = (float value) => Color.Lerp(Color.red, Color.green, value);
    protected Color defaultForegroundColor;

    public abstract void Init(float value);
    public abstract void SetValue01(float value);
    public abstract void SetValue01Instantly(float value);
}