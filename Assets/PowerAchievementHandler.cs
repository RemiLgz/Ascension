using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // N'oubliez pas d'ajouter l'import DoTween

public class PowerAchievementHandler : MonoBehaviour
{
    // R�f�rence au script PowerBarManager pour acc�der � playerPower
    public PowerBarManager powerBarManager;

    // Image � d�truire
    public Image imageToDestroy;

    // Panel � activer
    public GameObject panelToActivate;

    // Seuil de puissance � atteindre (modifiable dans l'inspecteur)
    public int powerThreshold = 8;

    // Bool�en pour v�rifier si le seuil de puissance a �t� atteint au moins une fois
    private bool hasReachedPowerThreshold = false;

    // Param�tres de l'animation DoTween
    public float imageShrinkDuration = 0.5f;  // Dur�e de la r�duction de l'image
    public float panelFadeInDuration = 0.5f;  // Dur�e de l'apparition du panneau

    // Variable pour enregistrer la taille initiale du panneau
    private Vector3 initialPanelScale;

    void Start()
    {
        // S'assurer que le panel est d�sactiv� au d�but
        if (panelToActivate != null)
        {
            // Enregistrer la taille initiale du panneau
            initialPanelScale = panelToActivate.transform.localScale;
            panelToActivate.SetActive(false);
        }
    }

    void Update()
    {
        // V�rifier si la valeur de playerPower atteint ou d�passe le seuil pour la premi�re fois
        if (!hasReachedPowerThreshold && powerBarManager.playerPower >= powerThreshold)
        {
            // Marquer comme atteint
            hasReachedPowerThreshold = true;

            // D�clencher l'animation de destruction de l'image
            if (imageToDestroy != null)
            {
                AnimateImageAndDestroy();
            }

            // D�clencher l'animation d'apparition du panel
            if (panelToActivate != null)
            {
                AnimatePanelAppearance();
            }
        }
    }

    void AnimateImageAndDestroy()
    {
        // R�duire l'image � 0 avec DoTween, puis la d�truire
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

        // Mettre l'alpha initial � 0 et l'�chelle � z�ro pour l'animation
        canvasGroup.alpha = 0;
        panelToActivate.transform.localScale = Vector3.zero;

        // Animation pour agrandir jusqu'� la taille initiale, avec fondu en m�me temps
        panelToActivate.transform.DOScale(initialPanelScale, panelFadeInDuration).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1, panelFadeInDuration);
    }
}
