using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionOnBossDefeat : MonoBehaviour
{
    [Header("References")]
    public Image fadeImage; // Image noire pour le fade
    public float fadeDuration = 1f; // Dur�e du fade

    [Header("Boss Health")]
    public ImageSwitcher imageSwitcher; // R�f�rence � l'objet qui contient la variable bossHealth

    public float delayBeforeSceneChange = 2f; // D�lai de 2 secondes avant de changer de sc�ne

    void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0); // S'assurer que l'image commence invisible
        }
    }

    void Update()
    {
        // V�rifie si la sant� du boss est inf�rieure � -1
        if (imageSwitcher.bossHealth < -1)
        {
            StartCoroutine(FadeAndChangeScene());
        }
    }

    // Coroutine pour le fade et changement de sc�ne
    private IEnumerator FadeAndChangeScene()
    {
        // D�marre le fondu au noir
        yield return StartCoroutine(FadeToBlack());

        // Attends 2 secondes sur l'�cran noir avant de changer de sc�ne
        yield return new WaitForSeconds(delayBeforeSceneChange);

        // Charge la sc�ne suivante (par exemple "SceneName" doit �tre remplac� par le nom r�el de votre sc�ne)
        SceneManager.LoadScene("End");
    }

    // Coroutine pour g�rer le fondu au noir
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

        // S'assurer que l'image est compl�tement noire � la fin du fade
        fadeImage.color = Color.black;
    }
}
