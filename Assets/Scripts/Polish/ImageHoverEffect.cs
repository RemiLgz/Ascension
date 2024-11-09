using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ImageHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("GameObject à faire apparaître/disparaître")]
    public GameObject objectToFade; // Le GameObject principal avec l'image et ses enfants

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f; // Durée du fade
    public float targetAlpha = 1f; // Alpha à atteindre (visible)
    public float startAlpha = 0f; // Alpha initial (invisible)

    void Start()
    {
        // S'assurer que le GameObject commence invisible (en mettant l'alpha de l'image et ses enfants à startAlpha)
        SetAlphaRecursively(objectToFade, startAlpha);
    }

    // Méthode appelée lorsque la souris entre sur l'objet
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Fade-in de l'objet et de tous ses enfants
        FadeObjectAndChildren(objectToFade, targetAlpha);
    }

    // Méthode appelée lorsque la souris quitte l'objet
    public void OnPointerExit(PointerEventData eventData)
    {
        // Fade-out de l'objet et de tous ses enfants
        FadeObjectAndChildren(objectToFade, startAlpha);
    }

    // Applique le fade à l'objet et à ses enfants
    void FadeObjectAndChildren(GameObject obj, float targetAlpha)
    {
        // Fade de l'objet principal
        if (obj != null)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>(); // Ajouter un CanvasGroup si nécessaire
            }

            canvasGroup.DOFade(targetAlpha, fadeDuration).SetEase(Ease.Linear);
        }

        // Fade des enfants
        foreach (Transform child in obj.transform)
        {
            FadeObjectAndChildren(child.gameObject, targetAlpha); // Appel récursif sur les enfants
        }
    }

    // Change l'alpha de l'objet et de tous ses enfants (initialisation à startAlpha)
    void SetAlphaRecursively(GameObject obj, float alpha)
    {
        // Change l'alpha de l'objet principal
        if (obj != null)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>(); // Ajouter un CanvasGroup si nécessaire
            }

            canvasGroup.alpha = alpha;
        }

        // Change l'alpha des enfants
        foreach (Transform child in obj.transform)
        {
            SetAlphaRecursively(child.gameObject, alpha); // Appel récursif sur les enfants
        }
    }
}
