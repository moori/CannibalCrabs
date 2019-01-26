using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image fillImage;
    public Gradient gradient;

    private CanvasGroup canvasGroup;

    public void UpdateFillBar(float value)
    {
        fillImage.fillAmount = value;
        fillImage.color = gradient.Evaluate(value);
        canvasGroup.DOFade(1, 0.1f);
        fillImage.DOFillAmount(value, 0.5f);
        canvasGroup.DOFade(0, 0.2f).SetDelay(2f);
    }
}
