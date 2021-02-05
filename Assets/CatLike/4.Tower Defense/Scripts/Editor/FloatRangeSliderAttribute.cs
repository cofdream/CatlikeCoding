using UnityEngine;
using UnityEditor;

public class FloatRangeSliderAttribute : PropertyAttribute
{
    public float Min { get; set; }
    public float Max { get; set; }

    public FloatRangeSliderAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}