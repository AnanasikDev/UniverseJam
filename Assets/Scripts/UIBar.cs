using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image foreground;
    [SerializeField][ReadOnly] private float width;

    [ReadOnly][ShowInInspector] public float Value { get; private set; } = 1.0f;
    [SerializeField][OnValueChanged("SetDoAnimateColor")] bool doAnimateForegroundColor = true;
    public Func<float, Color> GetForegroundColor = (float value) => Color.Lerp(Color.red, Color.green, value);
    private Color defaultForegroundColor;

    private void SetDoAnimateColor()
    {
        if (Application.isPlaying && !doAnimateForegroundColor && foreground)
        {
            foreground.color = defaultForegroundColor;
        }
    }

    public void Init()
    {
        Assert.AreApproximatelyEqual(background.rectTransform.sizeDelta.x, foreground.rectTransform.sizeDelta.x, "UIBar background and foreground must have identical width");
        width = background.rectTransform.sizeDelta.x;
        defaultForegroundColor = foreground.color;
    }

    public void SetValue01(float value)
    {
        Value = value;
        foreground.transform.localPosition = new Vector3(width * (Value - 1.0f), foreground.transform.localPosition.y);
        if (doAnimateForegroundColor) foreground.color = GetForegroundColor(Value);
    }
}