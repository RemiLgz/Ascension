using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    public Button quitButton;

    void Start()
    {
        quitButton.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        // Si on est en mode �diteur, arr�ter l'�diteur Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
