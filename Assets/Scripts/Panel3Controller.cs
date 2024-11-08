using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Panel3Controller : MonoBehaviour
{
    // R�f�rence au panel et aux images
    public GameObject panel;
    public Image[] images;

    // Dur�es et param�tres d'animation
    public float panelFadeInDuration = 0.5f;
    public float imageAppearDelay = 0.2f;
    public float imageScaleDuration = 0.3f;

    private void Start()
    {
        // Assure que le panel et les images sont pr�ts pour l'animation
        PreparePanelAndImages();
    }

    // Pr�pare le panel et cache les images au d�but
    private void PreparePanelAndImages()
    {
        // Cacher le panel
        panel.SetActive(false);

        // Mettre les images en invisible au d�but (taille r�duite et transparence � 0)
        foreach (var img in images)
        {
            img.transform.localScale = Vector3.zero;  // �chelle � 0 (invisible)
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);  // Transparence � 0
        }
    }

    // M�thode pour afficher Panel3 avec les animations "juicy"
    public void ShowPanel3()
    {
        // Activer le panel
        panel.SetActive(true);

        // Animer le panel avec une transition de fondu
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, panelFadeInDuration);

        // Animer chaque image une apr�s l'autre
        for (int i = 0; i < images.Length; i++)
        {
            Image img = images[i];

            // D�finir un d�lai pour chaque image
            float delay = panelFadeInDuration + (imageAppearDelay * i);

            // Animer la transparence (de 0 � 1) et l'�chelle pour l'effet punchy
            img.DOFade(1, imageScaleDuration).SetDelay(delay);  // Apparition progressive
            img.transform.DOScale(1.2f, imageScaleDuration).SetDelay(delay).SetEase(Ease.OutBack)
                .OnComplete(() => img.transform.DOScale(1f, 0.2f));  // Retour � l'�chelle normale
        }
    }
}
