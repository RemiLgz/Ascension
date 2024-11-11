using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AttractAttentionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button targetButton;
    public float punchScale = 0.2f;
    public int punchVibrato = 5;
    public float punchDuration = 0.8f;
    public float brightnessDuration = 0.5f;
    public float pauseDuration = 1.5f;

    private Image buttonImage;
    private Color originalColor;
    private Vector3 initialScale;
    private Sequence attentionSequence;

    void Start()
    {
        buttonImage = targetButton.GetComponent<Image>();
        initialScale = targetButton.transform.localScale;

        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
            StartAttentionAnimation();
        }
        else
        {
            Debug.LogError("Aucune image trouvée sur le bouton.");
        }
    }

    void StartAttentionAnimation()
    {
        attentionSequence = DOTween.Sequence();

        attentionSequence.Append(targetButton.transform.DOPunchScale(initialScale * punchScale, punchDuration, punchVibrato).SetEase(Ease.OutQuad));
        attentionSequence.Insert(0, buttonImage.DOColor(originalColor * 1.2f, brightnessDuration).SetLoops(2, LoopType.Yoyo));
        attentionSequence.AppendInterval(pauseDuration);

        attentionSequence.SetLoops(-1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        attentionSequence.Pause();
        targetButton.transform.localScale = initialScale;
        buttonImage.color = originalColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        attentionSequence.Play();
    }
}
