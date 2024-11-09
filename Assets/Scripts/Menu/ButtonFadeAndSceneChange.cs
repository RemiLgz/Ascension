using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections; // Ajoutez cette ligne pour les coroutines

public class ButtonFadeAndSceneChange : MonoBehaviour
{
    public Button buttonToFade;           // Le bouton qui sera cliqué
    public TextMeshProUGUI textToFade;    // Le texte qui sera animé
    public float fadeDuration = 1f;       // Durée du fade
    public float delayBeforeSceneChange = 2f; // Délai avant de changer de scène
    public string sceneName = "NewScene"; // Nom de la scène à charger

    private void Start()
    {
        // S'assurer que le bouton appelle la fonction "OnButtonClick" quand on clique dessus
        buttonToFade.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Lancer le fade pour le texte et le bouton
        FadeOutElements();

        // Appeler la fonction pour changer de scène après un délai
        StartCoroutine(ChangeSceneAfterDelay());
    }

    private void FadeOutElements()
    {
        // Faire disparaître le texte avec un effet de fade
        if (textToFade != null)
        {
            textToFade.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        }

        // Faire disparaître le bouton avec un effet de fade
        if (buttonToFade != null)
        {
            buttonToFade.GetComponent<Image>().DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
            buttonToFade.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        // Attendre pendant le délai
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Changer de scène
        SceneManager.LoadScene(sceneName);
    }
}
