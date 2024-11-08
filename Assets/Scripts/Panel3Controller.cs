using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel3Controller : MonoBehaviour
{
    // Référence au panel et aux images
    public GameObject panel;
    public Image[] images;

    // Durées et paramètres d'animation
    public float panelFadeInDuration = 0.5f;
    public float imageAppearDelay = 0.2f;
    public float imageScaleDuration = 0.3f;

    private void Start()
    {
        // Assure que le panel et les images sont prêts pour l'animation
        PreparePanelAndImages();
    }

    // Prépare le panel et cache les images au début
    private void PreparePanelAndImages()
    {
        // Cacher le panel
        panel.SetActive(false);

        // Mettre les images en invisible au début (taille réduite et transparence à 0)
        foreach (var img in images)
        {
            img.transform.localScale = Vector3.zero;  // Échelle à 0 (invisible)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);  // Transparence à 0
        }
    }

    // Méthode pour afficher Panel3 avec les animations "juicy"
    public void ShowPanel3()
    {
        // Activer le panel
        panel.SetActive(true);

        // Animer le panel avec une transition de fondu
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, panelFadeInDuration);

        // Animer chaque image une après l'autre
        for (int i = 0; i < images.Length; i++)
        {
            Image img = images[i];

            // Définir un délai pour chaque image
            float delay = panelFadeInDuration + (imageAppearDelay * i);

            // Animer la transparence (de 0 à 1) et l'échelle pour l'effet punchy
            img.DOFade(1, imageScaleDuration).SetDelay(delay);  // Apparition progressive
            img.transform.DOScale(1.2f, imageScaleDuration).SetDelay(delay).SetEase(Ease.OutBack)
                .OnComplete(() => img.transform.DOScale(1f, 0.2f));  // Retour à l'échelle normale
        }
    }
}
