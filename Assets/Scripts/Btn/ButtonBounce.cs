using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // Nécessaire pour DOTween

public class ButtonBounce : MonoBehaviour
{
    public Button button;  // Le bouton à animer
    private Vector3 originalScale;  // La taille d'origine du bouton

    public float bounceDuration = 0.1f;  // Durée plus rapide pour l'animation
    public float bounceStrength = 1.8f;  // Force du rebond (taille maximale)
    public float returnDuration = 0.1f;  // Durée pour revenir à la taille d'origine

    private void Start()
    {
        // Stocker la taille d'origine du bouton
        originalScale = button.transform.localScale;

        // Ajouter l'événement pour gérer le clic du bouton
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Créer un effet de rebond plus rapide et plus marqué
        BounceButton();
    }

    private void BounceButton()
    {
        // Créer une séquence DOTween
        Sequence bounceSequence = DOTween.Sequence();

        // Rebondir rapidement en augmentant la taille du bouton avec un effet marqué
        bounceSequence.Append(button.transform.DOScale(bounceStrength, bounceDuration).SetEase(Ease.OutQuad));

        // Faire revenir le bouton à sa taille originale rapidement
        bounceSequence.Append(button.transform.DOScale(originalScale, returnDuration).SetEase(Ease.InOutQuad));
    }
}
