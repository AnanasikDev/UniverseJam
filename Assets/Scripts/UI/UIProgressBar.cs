using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class UIProgressBar : AbstractUIProgress
{
    [SerializeField][ReadOnly] private float width;
    
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
        foreground.transform.localPosition = new Vector3(width * (Value - 1.0f), foreground.transform.localPosition.y);
        if (doAnimateForegroundColor) foreground.color = GetForegroundColor(Value);
    }

    public override void SetValue01(float value)
    {
        float prevValue = Value;
        Value = value;
        float diff = Value - prevValue;
        if (diff == 0) return; // nothing changed;

        foreground.transform.DOLocalMoveX(width * (Value - 1.0f), Mathf.Sqrt(Mathf.Abs(diff)));
        if (doAnimateForegroundColor) foreground.color = GetForegroundColor(Value);
    }
}