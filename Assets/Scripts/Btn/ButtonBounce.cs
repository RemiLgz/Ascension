using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // N�cessaire pour DOTween

public class ButtonBounce : MonoBehaviour
{
    public Button button;  // Le bouton � animer
    private Vector3 originalScale;  // La taille d'origine du bouton

    public float bounceDuration = 0.1f;  // Dur�e plus rapide pour l'animation
    public float bounceStrength = 1.8f;  // Force du rebond (taille maximale)
    public float returnDuration = 0.1f;  // Dur�e pour revenir � la taille d'origine

    private void Start()
    {
        // Stocker la taille d'origine du bouton
        originalScale = button.transform.localScale;

        // Ajouter l'�v�nement pour g�rer le clic du bouton
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Cr�er un effet de rebond plus rapide et plus marqu�
        BounceButton();
    }

    private void BounceButton()
    {
        // Cr�er une s�quence DOTween
        Sequence bounceSequence = DOTween.Sequence();

        // Rebondir rapidement en augmentant la taille du bouton avec un effet marqu�
        bounceSequence.Append(button.transform.DOScale(bounceStrength, bounceDuration).SetEase(Ease.OutQuad));

        // Faire revenir le bouton � sa taille originale rapidement
        bounceSequence.Append(button.transform.DOScale(originalScale, returnDuration).SetEase(Ease.InOutQuad));
    }
}
