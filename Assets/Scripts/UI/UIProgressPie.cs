using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class UIProgressPie : AbstractUIProgress
{
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
        defaultForegroundColor = foreground.color;
        SetValue01Instantly(value);
    }

    public override void SetValue01Instantly(float value)
    {
        Value = value;
        foreground.fillAmount = Value;
        if (doAnimateForegroundColor) foreground.color = GetForegroundColor(Value);
    }

    public override void SetValue01(float value)
    {
        float prevValue = Value;
        Value = value;
        float diff = Value - prevValue;
        if (diff == 0) return; // nothing changed;

        foreground.DOFillAmount(Value, Mathf.Sqrt(Mathf.Abs(diff)));
        if (doAnimateForegroundColor) foreground.color = GetForegroundColor(Value);
    }
}