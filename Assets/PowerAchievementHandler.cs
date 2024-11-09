using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // N'oubliez pas d'ajouter l'import DoTween

public class PowerAchievementHandler : MonoBehaviour
{
    // Référence au script PowerBarManager pour accéder à playerPower
    public PowerBarManager powerBarManager;

    // Image à détruire
    public Image imageToDestroy;

    // Panel à activer
    public GameObject panelToActivate;

    // Seuil de puissance à atteindre (modifiable dans l'inspecteur)
    public int powerThreshold = 8;

    // Booléen pour vérifier si le seuil de puissance a été atteint au moins une fois
    private bool hasReachedPowerThreshold = false;

    // Paramètres de l'animation DoTween
    public float imageShrinkDuration = 0.5f;  // Durée de la réduction de l'image
    public float panelFadeInDuration = 0.5f;  // Durée de l'apparition du panneau

    // Variable pour enregistrer la taille initiale du panneau
    private Vector3 initialPanelScale;

    void Start()
    {
        // S'assurer que le panel est désactivé au début
        if (panelToActivate != null)
        {
            // Enregistrer la taille initiale du panneau
            initialPanelScale = panelToActivate.transform.localScale;
            panelToActivate.SetActive(false);
        }
    }

    void Update()
    {
        // Vérifier si la valeur de playerPower atteint ou dépasse le seuil pour la première fois
        if (!hasReachedPowerThreshold && powerBarManager.playerPower >= powerThreshold)
        {
            // Marquer comme atteint
            hasReachedPowerThreshold = true;

            // Déclencher l'animation de destruction de l'image
            if (imageToDestroy != null)
            {
                AnimateImageAndDestroy();
            }

            // Déclencher l'animation d'apparition du panel
            if (panelToActivate != null)
            {
                AnimatePanelAppearance();
            }
        }
    }

    void AnimateImageAndDestroy()
    {
        // Réduire l'image à 0 avec DoTween, puis la détruire
        imageToDestroy.rectTransform.DOScale(Vector3.zero, imageShrinkDuration)
            .SetEase(Ease.InBack) // Style de transition
            .OnComplete(() => Destroy(imageToDestroy.gameObject));
    }

    void AnimatePanelAppearance()
    {
        // Activer le panel puis faire une animation d'apparition (ex : scale + fade-in)
        panelToActivate.SetActive(true);
        CanvasGroup canvasGroup = panelToActivate.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Ajouter un CanvasGroup si inexistant
            canvasGroup = panelToActivate.AddComponent<CanvasGroup>();
        }

        // Mettre l'alpha initial à 0 et l'échelle à zéro pour l'animation
        canvasGroup.alpha = 0;
        panelToActivate.transform.localScale = Vector3.zero;

        // Animation pour agrandir jusqu'à la taille initiale, avec fondu en même temps
        panelToActivate.transform.DOScale(initialPanelScale, panelFadeInDuration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1, panelFadeInDuration);
    }
}
