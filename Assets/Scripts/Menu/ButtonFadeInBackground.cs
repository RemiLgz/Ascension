using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonFadeInTextWithBackground : MonoBehaviour
{
    public Image blackScreen;               // Image de fond noir
    public TextMeshProUGUI messageText;     // Texte à afficher
    public Button actionButton;             // Bouton à afficher
    public float fadeDuration = 0.5f;       // Durée du fondu pour le fond noir
    public float textDelay = 1.0f;          // Délai avant l'apparition du texte et du bouton
    public float textFadeDuration = 1.0f;   // Durée du fondu pour le texte et le bouton

    private void Start()
    {
        // Initialiser l'opacité du fond noir, du texte, et du bouton à 0
        if (blackScreen != null)
        {
            blackScreen.color = new Color(0, 0, 0, 0); // Fond transparent au départ
        }

        if (messageText != null)
        {
            messageText.alpha = 0; // Opacité du texte à 0
        }

        if (actionButton != null)
        {
            actionButton.GetComponentInChildren<TextMeshProUGUI>().alpha = 0; // Opacité du texte du bouton à 0
            actionButton.image.color = new Color(1, 1, 1, 0); // Opacité de l'image du bouton à 0
        }
        else
        {
            Debug.LogWarning("Action Button n'est pas assigné dans l'inspecteur !");
        }
    }

    public void OnButtonPress()
    {
        // Faire apparaître le fond noir en fondu complet (opaque)
        if (blackScreen != null)
        {
            blackScreen.DOFade(1f, fadeDuration)
                       .SetEase(Ease.InOutQuad)
                       .OnComplete(() => FadeInTextAndButton());
        }
    }

    private void FadeInTextAndButton()
    {
        // Attendre un délai avant de lancer le fondu du texte et du bouton
        DOVirtual.DelayedCall(textDelay, () =>
        {
            if (messageText != null)
            {
                // Faire apparaître le texte avec un effet de fondu
                messageText.DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);
            }

            if (actionButton != null)
            {
                // Faire apparaître le bouton avec un effet de fondu
                actionButton.image.DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);

                // Faire apparaître le texte du bouton
                actionButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);
            }
        });
    }
}
