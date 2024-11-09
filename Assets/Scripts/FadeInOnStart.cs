using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOnStart : MonoBehaviour
{
    [Header("References")]
    public Image fadeImage; // Image noire pour le fade
    public float fadeDuration = 1f; // Durée du fade

    void Start()
    {
        // Lance la coroutine pour effectuer le fondu au noir
        StartCoroutine(FadeOut());
    }

    // Coroutine pour effectuer le fade (de 1 à 0)
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        // Graduellement change l'alpha de l'image pour la rendre transparente
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(1 - (elapsedTime / fadeDuration)));
            yield return null;
        }

        // S'assurer que l'image est complètement transparente à la fin du fade
        fadeImage.color = new Color(0, 0, 0, 0);
    }
}
