using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ImageHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("GameObject � faire appara�tre/dispara�tre")]
    public GameObject objectToFade; // Le GameObject principal avec l'image et ses enfants

    [Header("Fade Settings")]
    public float fadeDuration = 0.5f; // Dur�e du fade
    public float targetAlpha = 1f; // Alpha � atteindre (visible)
    public float startAlpha = 0f; // Alpha initial (invisible)

    void Start()
    {
        // S'assurer que le GameObject commence invisible (en mettant l'alpha de l'image et ses enfants � startAlpha)
        SetAlphaRecursively(objectToFade, startAlpha);
    }

    // M�thode appel�e lorsque la souris entre sur l'objet
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Fade-in de l'objet et de tous ses enfants
        FadeObjectAndChildren(objectToFade, targetAlpha);
    }

    // M�thode appel�e lorsque la souris quitte l'objet
    public void OnPointerExit(PointerEventData eventData)
    {
        // Fade-out de l'objet et de tous ses enfants
        FadeObjectAndChildren(objectToFade, startAlpha);
    }

    // Applique le fade � l'objet et � ses enfants
    void FadeObjectAndChildren(GameObject obj, float targetAlpha)
    {
        // Fade de l'objet principal
        if (obj != null)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>(); // Ajouter un CanvasGroup si n�cessaire
            }

            canvasGroup.DOFade(targetAlpha, fadeDuration).SetEase(Ease.Linear);
        }

        // Fade des enfants
        foreach (Transform child in obj.transform)
        {
            FadeObjectAndChildren(child.gameObject, targetAlpha); // Appel r�cursif sur les enfants
        }
    }

    // Change l'alpha de l'objet et de tous ses enfants (initialisation � startAlpha)
    void SetAlphaRecursively(GameObject obj, float alpha)
    {
        // Change l'alpha de l'objet principal
        if (obj != null)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>(); // Ajouter un CanvasGroup si n�cessaire
            }

            canvasGroup.alpha = alpha;
        }

        // Change l'alpha des enfants
        foreach (Transform child in obj.transform)
        {
            SetAlphaRecursively(child.gameObject, alpha); // Appel r�cursif sur les enfants
        }
    }
}
