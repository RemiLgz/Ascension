using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ButtonFadeInTextWithBackground : MonoBehaviour
{
    public Image blackScreen;               // Image de fond noir
    public TextMeshProUGUI messageText;     // Texte � afficher
    public Button actionButton;             // Bouton � afficher
    public float fadeDuration = 0.5f;       // Dur�e du fondu pour le fond noir
    public float textDelay = 1.0f;          // D�lai avant l'apparition du texte et du bouton
    public float textFadeDuration = 1.0f;   // Dur�e du fondu pour le texte et le bouton

    private void Start()
    {
        // Initialiser l'opacit� du fond noir, du texte, et du bouton � 0
        if (blackScreen != null)
        {
            blackScreen.color = new Color(0, 0, 0, 0); // Fond transparent au d�part
        }

        if (messageText != null)
        {
            messageText.alpha = 0; // Opacit� du texte � 0
        }

        if (actionButton != null)
        {
            actionButton.GetComponentInChildren<TextMeshProUGUI>().alpha = 0; // Opacit� du texte du bouton � 0
            actionButton.image.color = new Color(1, 1, 1, 0); // Opacit� de l'image du bouton � 0
        }
        else
        {
            Debug.LogWarning("Action Button n'est pas assign� dans l'inspecteur !");
        }
    }

    public void OnButtonPress()
    {
        // Faire appara�tre le fond noir en fondu complet (opaque)
        if (blackScreen != null)
        {
            blackScreen.DOFade(1f, fadeDuration)
                       .SetEase(Ease.InOutQuad)
                       .OnComplete(() => FadeInTextAndButton());
        }
    }

    private void FadeInTextAndButton()
    {
        // Attendre un d�lai avant de lancer le fondu du texte et du bouton
        DOVirtual.DelayedCall(textDelay, () =>
        {
            if (messageText != null)
            {
                // Faire appara�tre le texte avec un effet de fondu
                messageText.DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);
            }

            if (actionButton != null)
            {
                // Faire appara�tre le bouton avec un effet de fondu
                actionButton.image.DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);

                // Faire appara�tre le texte du bouton
                actionButton.GetComponentInChildren<TextMeshProUGUI>().DOFade(1, textFadeDuration).SetEase(Ease.InOutQuad);
            }
        });
    }
}
