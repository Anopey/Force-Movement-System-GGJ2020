using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public float maxValue, minValue, value;
    public RectTransform childImage;

    public void HealtSet(float persantage)
    {
        value = ((maxValue - minValue) * persantage / 100) + minValue;
        childImage.sizeDelta = new Vector2(value, 80);
    }
}
