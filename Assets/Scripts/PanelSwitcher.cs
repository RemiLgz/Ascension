using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelSwitcher : MonoBehaviour
{
    public PowerBarManager powerBarManager;  // R�f�rence au script PowerBarManager
    public GameObject panelToClose;          // Panel � fermer
    public GameObject panelToOpen;           // Panel � ouvrir
    public Button switchButton;              // Bouton pour d�clencher l'action
    public int prix = 100;                   // Prix requis pour l'action
    public float animationDuration = 0.5f;   // Dur�e de l'animation pour les transitions

    private Vector3 panelOriginalScale;      // Stocke la taille d'origine du panel � ouvrir

    void Start()
    {
        // Sauvegarder la taille initiale du panel � ouvrir
        panelOriginalScale = panelToOpen.transform.localScale;

        // Relier le bouton � la fonction SwitchPanels
        switchButton.onClick.AddListener(SwitchPanels);

        // Initialiser l'�tat du bouton
        UpdateButtonInteractivity();
    }

    void Update()
    {
        // Mettre � jour l'apparence du bouton en fonction du playerPower
        UpdateButtonInteractivity();
    }

    // M�thode pour v�rifier le playerPower et switcher les panels
    void SwitchPanels()
    {
        if (powerBarManager.playerPower >= prix)
        {
            // Soustraire le prix du playerPower
            powerBarManager.playerPower -= prix;

            // Fermer le panel actuel de mani�re "juicy"
            panelToClose.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    // D�sactiver le panel une fois l'animation termin�e
                    panelToClose.SetActive(false);

                    // Activer le nouveau panel et jouer une animation d'ouverture
                    panelToOpen.SetActive(true);
                    panelToOpen.transform.localScale = Vector3.zero;  // D�finir l'�chelle de d�part � z�ro
                    panelToOpen.transform.DOScale(panelOriginalScale, animationDuration).SetEase(Ease.OutBack);
                });
        }
        else
        {
            Debug.Log("Pas assez de playerPower pour effectuer cette action.");
        }
    }

    // Mettre � jour l'opacit� et l'interactivit� du bouton en fonction de playerPower
    void UpdateButtonInteractivity()
    {
        if (powerBarManager.playerPower < prix)
        {
            // Rendre le bouton inactif avec une opacit� r�duite
            switchButton.interactable = false;
            switchButton.image.color = new Color(switchButton.image.color.r, switchButton.image.color.g, switchButton.image.color.b, 0.4f);
        }
        else
        {
            // Rendre le bouton actif avec une opacit� pleine
            switchButton.interactable = true;
            switchButton.image.color = new Color(switchButton.image.color.r, switchButton.image.color.g, switchButton.image.color.b, 1f);
        }
    }
}
