using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionOnBossDefeat : MonoBehaviour
{
    [Header("References")]
    public Image fadeImage; // Image noire pour le fade
    public float fadeDuration = 1f; // Durée du fade

    [Header("Boss Health")]
    public ImageSwitcher imageSwitcher; // Référence à l'objet qui contient la variable bossHealth

    public float delayBeforeSceneChange = 2f; // Délai de 2 secondes avant de changer de scène

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // S'assurer que l'image commence invisible
        }
    }

    void Update()
    {
        // Vérifie si la santé du boss est inférieure à -1
        if (imageSwitcher.bossHealth < -1)
        {
            StartCoroutine(FadeAndChangeScene());
        }
    }

    // Coroutine pour le fade et changement de scène
    private IEnumerator FadeAndChangeScene()
    {
        // Démarre le fondu au noir
        yield return StartCoroutine(FadeToBlack());

        // Attends 2 secondes sur l'écran noir avant de changer de scène
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Charge la scène suivante (par exemple "SceneName" doit être remplacé par le nom réel de votre scène)
        SceneManager.LoadScene("End");
    }

    // Coroutine pour gérer le fondu au noir
    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        // Graduellement change l'alpha de l'image pour la rendre noire
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeDuration));
            yield return null;
        }

        // S'assurer que l'image est complètement noire à la fin du fade
        fadeImage.color = Color.black;
    }
}
