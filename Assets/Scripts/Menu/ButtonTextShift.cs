using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonTextShift : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float shiftDistance = 30f;      // Distance de décalage vers la droite
    public float animationDuration = 0.3f; // Durée de l'animation en secondes

    private TextMeshProUGUI buttonText;
    private Vector3 originalPosition;

    void Start()
    {
        // Récupérer le texte du bouton
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        // Vérifier que le texte est bien présent
        if (buttonText != null)
        {
            // Enregistrer la position initiale du texte
            originalPosition = buttonText.rectTransform.localPosition;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI introuvable en tant qu'enfant de ce bouton.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Décaler le texte vers la droite avec DoTween
        if (buttonText != null)
        {
            buttonText.rectTransform.DOLocalMoveX(originalPosition.x + shiftDistance, animationDuration)
                                     .SetEase(Ease.OutQuad);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Ramener le texte à sa position initiale avec DoTween
        if (buttonText != null)
        {
            buttonText.rectTransform.DOLocalMoveX(originalPosition.x, animationDuration)
                                     .SetEase(Ease.OutQuad);
        }
    }
}
