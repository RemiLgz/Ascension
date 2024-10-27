using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    // Références aux deux panels
    public GameObject panel1;
    public GameObject panel2;

    // Références aux boutons
    public Button buttonAmelioration;
    public Button buttonQuitter;

    // Durées et paramètres d'animation
    public float fadeDuration = 0.5f;
    public float moveDistance = 50f;
    public float delayBetweenPanels = 0.2f;

    private CanvasGroup canvasGroup1;
    private CanvasGroup canvasGroup2;

    // Positions d'origine des panels
    private Vector3 panel1OriginalPosition;
    private Vector3 panel2OriginalPosition;

    private void Start()
    {
        // Assure que le bouton "Quitter" est désactivé au démarrage
        buttonQuitter.gameObject.SetActive(false);

        // Assigne les événements de clic pour chaque bouton
        buttonAmelioration.onClick.AddListener(() => { BounceButton(buttonAmelioration, ShowPanels); });
        buttonQuitter.onClick.AddListener(() => { BounceButton(buttonQuitter, ClosePanels); });

        // Initialisation des CanvasGroups pour contrôler l'opacité
        canvasGroup1 = panel1.GetComponent<CanvasGroup>() ?? panel1.AddComponent<CanvasGroup>();
        canvasGroup2 = panel2.GetComponent<CanvasGroup>() ?? panel2.AddComponent<CanvasGroup>();

        // Cache les panels au démarrage
        panel1.SetActive(false);
        panel2.SetActive(false);

        // Enregistre les positions d'origine des panels
        panel1OriginalPosition = panel1.transform.localPosition;
        panel2OriginalPosition = panel2.transform.localPosition;
    }

    // Méthode pour afficher les panels et gérer les boutons
    public void ShowPanels()
    {
        // Réinitialise la position et l'opacité des panels
        panel1.transform.localPosition = panel1OriginalPosition - new Vector3(moveDistance, 0, 0);
        panel2.transform.localPosition = panel2OriginalPosition - new Vector3(moveDistance, 0, 0);
        canvasGroup1.alpha = 0f;
        canvasGroup2.alpha = 0f;

        // Active les panels avant l'animation
        panel1.SetActive(true);
        panel2.SetActive(true);

        // Animation du premier panel avec un rebond
        canvasGroup1.DOFade(1f, fadeDuration);
        panel1.transform.DOLocalMoveX(panel1OriginalPosition.x, fadeDuration)
                        .SetEase(Ease.OutBack); // Effet de rebond à la fin

        // Animation du second panel avec décalage et un rebond
        canvasGroup2.DOFade(1f, fadeDuration).SetDelay(delayBetweenPanels);
        panel2.transform.DOLocalMoveX(panel2OriginalPosition.x, fadeDuration)
                        .SetEase(Ease.OutBack) // Effet de rebond à la fin
                        .SetDelay(delayBetweenPanels);

        // Change les boutons après une petite attente pour permettre le rebond
        DOVirtual.DelayedCall(0.3f, () =>
        {
            buttonAmelioration.gameObject.SetActive(false);
            buttonQuitter.gameObject.SetActive(true);
        });
    }

    // Méthode pour fermer les panels et remettre le bouton "Amélioration"
    public void ClosePanels()
    {
        // Animation de fermeture du premier panel
        canvasGroup1.DOFade(0f, fadeDuration);
        panel1.transform.DOLocalMoveX(panel1OriginalPosition.x - moveDistance, fadeDuration)
                        .SetEase(Ease.InBack); // Effet de rebond

        // Animation de fermeture du second panel avec décalage
        canvasGroup2.DOFade(0f, fadeDuration).SetDelay(delayBetweenPanels);
        panel2.transform.DOLocalMoveX(panel2OriginalPosition.x - moveDistance, fadeDuration)
                        .SetEase(Ease.InBack) // Effet de rebond
                        .SetDelay(delayBetweenPanels);

        // Réinitialise les boutons après le rebond du bouton "Quitter"
        DOVirtual.DelayedCall(0.3f, () =>
        {
            buttonQuitter.gameObject.SetActive(false);
            buttonAmelioration.gameObject.SetActive(true);
        });
    }

    // Effet de rebond pour les boutons avec une action de callback
    private void BounceButton(Button button, TweenCallback onComplete)
    {
        button.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0), 0.3f, 10, 1).OnComplete(onComplete);
    }
}
