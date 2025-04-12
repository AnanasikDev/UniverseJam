using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class UIProgressShaderBar : AbstractUIProgress
{
    [SerializeField][ReadOnly] private float width;
    [SerializeField] private string progressParam;
    [SerializeField] private string colorParam;

    private void SetDoAnimateColor()
    {
        if (Application.isPlaying && !doAnimateForegroundColor && foreground)
        {
            foreground.color = defaultForegroundColor;
        }
    }

    public override void Init(float value)
    {
        Assert.AreApproximatelyEqual(background.rectTransform.sizeDelta.x, foreground.rectTransform.sizeDelta.x, "UIBar background and foreground must have identical width");
        width = background.rectTransform.sizeDelta.x;
        defaultForegroundColor = foreground.color;
        SetValue01Instantly(value);
    }

    public override void SetValue01Instantly(float value)
    {
        Value = value;
        foreground.material.SetFloat(progressParam, value);
        if (doAnimateForegroundColor) foreground.material.SetColor(colorParam, GetForegroundColor(Value));
    }

    public override void SetValue01(float value)
    {
        float prevValue = Value;
        Value = value;
        float diff = Value - prevValue;
        if (diff == 0) return; // nothing changed;

        foreground.material.DOFloat(value, progressParam, Mathf.Sqrt(Mathf.Abs(diff)));

        if (doAnimateForegroundColor) foreground.material.DOColor(GetForegroundColor(Value), colorParam, Mathf.Sqrt(Mathf.Abs(diff)));
    }
}