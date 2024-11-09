using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionButton : MonoBehaviour
{
    public string targetSceneName;
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float holdBlackDuration = 1f;  // Durée du maintien du noir avant le changement de scène

    public void OnButtonClick()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        if (fadeImage == null)
        {
            Debug.LogError("fadeImage is not assigned in the Inspector.");
            yield break;
        }

        fadeImage.gameObject.SetActive(true);

        Color startColor = fadeImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = endColor;

        // Maintenir l'image noire pendant un certain temps avant de changer la scène
        yield return new WaitForSeconds(holdBlackDuration);

        Debug.Log("Fade complete! Loading scene: " + targetSceneName);

        SceneManager.LoadScene(targetSceneName);
    }
}
