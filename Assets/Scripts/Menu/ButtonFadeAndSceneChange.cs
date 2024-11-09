using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections; // Ajoutez cette ligne pour les coroutines

public class ButtonFadeAndSceneChange : MonoBehaviour
{
    public Button buttonToFade;           // Le bouton qui sera cliqu�
    public TextMeshProUGUI textToFade;    // Le texte qui sera anim�
    public float fadeDuration = 1f;       // Dur�e du fade
    public float delayBeforeSceneChange = 2f; // D�lai avant de changer de sc�ne
    public string sceneName = "NewScene"; // Nom de la sc�ne � charger

    private void Start()
    {
        // S'assurer que le bouton appelle la fonction "OnButtonClick" quand on clique dessus
        buttonToFade.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // Lancer le fade pour le texte et le bouton
        FadeOutElements();

        // Appeler la fonction pour changer de sc�ne apr�s un d�lai
        StartCoroutine(ChangeSceneAfterDelay());
    }

    private void FadeOutElements()
    {
        // Faire dispara�tre le texte avec un effet de fade
        if (textToFade != null)
        {
            textToFade.DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        }

        // Faire dispara�tre le bouton avec un effet de fade
        if (buttonToFade != null)
        {
            buttonToFade.GetComponent<Image>().DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
            buttonToFade.GetComponentInChildren<TextMeshProUGUI>().DOFade(0, fadeDuration).SetEase(Ease.InOutQuad);
        }
    }

    private IEnumerator ChangeSceneAfterDelay()
    {
        // Attendre pendant le d�lai
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Changer de sc�ne
        SceneManager.LoadScene(sceneName);
    }
}
