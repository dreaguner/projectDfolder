using UnityEngine;
using UnityEngine.UI;

public class TurnGaugeUI : MonoBehaviour
{
    public Image fillImage;

    public void SetGauge(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value / 100f);
    }
}
