using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image fillImage;
    public Gradient gradient;

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateFillBar(float value)
    {
        canvasGroup.DOKill();
        fillImage.DOColor(gradient.Evaluate(value), 0.5f);
        canvasGroup.DOFade(1, 0.1f);
        fillImage.DOFillAmount(value, 0.5f);
        canvasGroup.DOFade(0, 0.2f).SetDelay(2f);
    }
}
