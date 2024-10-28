using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    // R�f�rences aux deux panels
    public GameObject panel1;
    public GameObject panel2;

    // R�f�rences aux boutons
    public Button buttonAmelioration;
    public Button buttonQuitter;

    // Dur�es et param�tres d'animation
    public float fadeDuration = 0.5f;
    public float moveDistance = 50f;
    public float delayBetweenPanels = 0.2f;

    private CanvasGroup canvasGroup1;
    private CanvasGroup canvasGroup2;

    // Positions d'origine des panels
    private Vector3 panel1OriginalPosition;
    private Vector3 panel2OriginalPosition;

    // Bool�en pour v�rifier si une animation est en cours
    private bool isAnimating = false;

    private void Start()
    {
        // Assure que le bouton "Quitter" est d�sactiv� au d�marrage
        buttonQuitter.gameObject.SetActive(false);

        // Assigne les �v�nements de clic pour chaque bouton
        buttonAmelioration.onClick.AddListener(() => { BounceButton(buttonAmelioration, ShowPanels); });
        buttonQuitter.onClick.AddListener(() => { BounceButton(buttonQuitter, ClosePanels); });

        // Initialisation des CanvasGroups pour contr�ler l'opacit�
        canvasGroup1 = panel1.GetComponent<CanvasGroup>() ?? panel1.AddComponent<CanvasGroup>();
        canvasGroup2 = panel2.GetComponent<CanvasGroup>() ?? panel2.AddComponent<CanvasGroup>();

        // Cache les panels et enregistre leur position d'origine
        panel1.SetActive(false);
        panel2.SetActive(false);
        panel1OriginalPosition = panel1.transform.localPosition;
        panel2OriginalPosition = panel2.transform.localPosition;

        // R�initialise la position et l'opacit� au d�marrage
        ResetPanels();
    }

    // M�thode pour afficher les panels et g�rer les boutons
    public void ShowPanels()
    {
        // V�rifie si une animation est d�j� en cours
        if (isAnimating) return;
        isAnimating = true;

        // Active les panels avant l'animation
        panel1.SetActive(true);
        panel2.SetActive(true);

        // Animation du premier panel avec un rebond
        canvasGroup1.DOFade(1f, fadeDuration);
        panel1.transform.DOLocalMoveX(panel1OriginalPosition.x, fadeDuration)
                        .SetEase(Ease.OutBack); // Effet de rebond � la fin

        // Animation du second panel avec d�calage et un rebond
        canvasGroup2.DOFade(1f, fadeDuration).SetDelay(delayBetweenPanels);
        panel2.transform.DOLocalMoveX(panel2OriginalPosition.x, fadeDuration)
                        .SetEase(Ease.OutBack) // Effet de rebond � la fin
                        .SetDelay(delayBetweenPanels)
                        .OnComplete(() =>
                        {
                            // Termine l'animation et active le bouton "Quitter"
                            buttonAmelioration.gameObject.SetActive(false);
                            buttonQuitter.gameObject.SetActive(true);
                            isAnimating = false;
                        });
    }

    // M�thode pour fermer les panels et remettre le bouton "Am�lioration"
    public void ClosePanels()
    {
        // V�rifie si une animation est d�j� en cours
        if (isAnimating) return;
        isAnimating = true;

        // Animation de fermeture du premier panel
        canvasGroup1.DOFade(0f, fadeDuration);
        panel1.transform.DOLocalMoveX(panel1OriginalPosition.x - moveDistance, fadeDuration)
                        .SetEase(Ease.InBack); // Effet de rebond

        // Animation de fermeture du second panel avec d�calage
        canvasGroup2.DOFade(0f, fadeDuration).SetDelay(delayBetweenPanels);
        panel2.transform.DOLocalMoveX(panel2OriginalPosition.x - moveDistance, fadeDuration)
                        .SetEase(Ease.InBack) // Effet de rebond
                        .SetDelay(delayBetweenPanels)
                        .OnComplete(() =>
                        {
                            // D�sactive les panels apr�s la fermeture
                            panel1.SetActive(false);
                            panel2.SetActive(false);

                            // R�initialise les boutons et panels
                            buttonQuitter.gameObject.SetActive(false);
                            buttonAmelioration.gameObject.SetActive(true);
                            ResetPanels();
                            isAnimating = false;
                        });
    }

    // Effet de rebond pour les boutons avec une action de callback
    private void BounceButton(Button button, TweenCallback onComplete)
    {
        button.transform.DOPunchScale(new Vector3(0.15f, 0.15f, 0), 0.3f, 10, 1).OnComplete(onComplete);
    }

    // R�initialise les positions et l'opacit� des panels
    private void ResetPanels()
    {
        panel1.transform.localPosition = panel1OriginalPosition - new Vector3(moveDistance, 0, 0);
        panel2.transform.localPosition = panel2OriginalPosition - new Vector3(moveDistance, 0, 0);
        canvasGroup1.alpha = 0f;
        canvasGroup2.alpha = 0f;
    }
}
