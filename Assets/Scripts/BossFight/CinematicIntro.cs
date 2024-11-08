using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CinematicIntro : MonoBehaviour
{
    public Image blackScreen;
    public float fadeDuration = 1f;

    public Camera mainCamera;
    public Vector3 startCameraPosition;
    public Vector3 endCameraPosition;
    public float cameraDelay = 2f;
    public float cameraMoveDuration = 3f;

    public RectTransform topBlackBar;
    public RectTransform bottomBlackBar;
    public float barAnimationDuration = 1f;
    public float barWaitDuration = 2f;

    public RectTransform panelToShow; // Le panel que tu veux faire appara�tre
    public float panelAnimationDuration = 1f; // Dur�e de l'animation du panel

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 1);
        panelToShow.gameObject.SetActive(false); // D�sactive le panel au d�part
        StartCoroutine(PlayCinematic());
    }

    private IEnumerator PlayCinematic()
    {
        blackScreen.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(fadeDuration + cameraDelay);

        mainCamera.transform.position = startCameraPosition;
        mainCamera.transform.DOMove(endCameraPosition, cameraMoveDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(cameraMoveDuration + barWaitDuration);

        // Animation des bandes noires
        Sequence barExitSequence = DOTween.Sequence();
        barExitSequence.Append(topBlackBar.DOAnchorPosY(Screen.height / 2 + topBlackBar.rect.height, barAnimationDuration).SetEase(Ease.InQuad));
        barExitSequence.Join(bottomBlackBar.DOAnchorPosY(-Screen.height / 2 - bottomBlackBar.rect.height, barAnimationDuration).SetEase(Ease.InQuad));
        barExitSequence.OnComplete(ShowPanelDiscreetly); // Appeler la fonction pour afficher le panel de mani�re discr�te
    }

    // Fonction pour afficher le panel de mani�re discr�te
    private void ShowPanelDiscreetly()
    {
        panelToShow.gameObject.SetActive(true); // Active le panel avant de commencer l'animation

        // Commencer avec une �chelle r�duite sans changer l'opacit�
        panelToShow.localScale = Vector3.zero;

        // Animation douce du panel pour qu'il devienne visible en se d�ployant sans changer l'opacit�
        panelToShow.DOScale(Vector3.one, panelAnimationDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                // L'animation du panel est termin�e, il reste � sa position et taille finale
            });
    }
}
