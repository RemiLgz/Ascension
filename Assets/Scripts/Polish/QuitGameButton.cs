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
        // Si on est en mode éditeur, arrêter l'éditeur Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
