using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelSwitcher : MonoBehaviour
{
    public PowerBarManager powerBarManager;  // Référence au script PowerBarManager
    public GameObject panelToClose;          // Panel à fermer
    public GameObject panelToOpen;           // Panel à ouvrir
    public Button switchButton;              // Bouton pour déclencher l'action
    public int prix = 100;                   // Prix requis pour l'action
    public float animationDuration = 0.5f;   // Durée de l'animation pour les transitions

    private Vector3 panelOriginalScale;      // Stocke la taille d'origine du panel à ouvrir

    void Start()
    {
        // Sauvegarder la taille initiale du panel à ouvrir
        panelOriginalScale = panelToOpen.transform.localScale;

        // Relier le bouton à la fonction SwitchPanels
        switchButton.onClick.AddListener(SwitchPanels);

        // Initialiser l'état du bouton
        UpdateButtonInteractivity();
    }

    void Update()
    {
        // Mettre à jour l'apparence du bouton en fonction du playerPower
        UpdateButtonInteractivity();
    }

    // Méthode pour vérifier le playerPower et switcher les panels
    void SwitchPanels()
    {
        if (powerBarManager.playerPower >= prix)
        {
            // Soustraire le prix du playerPower
            powerBarManager.playerPower -= prix;

            // Fermer le panel actuel de manière "juicy"
            panelToClose.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    // Désactiver le panel une fois l'animation terminée
                    panelToClose.SetActive(false);

                    // Activer le nouveau panel et jouer une animation d'ouverture
                    panelToOpen.SetActive(true);
                    panelToOpen.transform.localScale = Vector3.zero;  // Définir l'échelle de départ à zéro
                    panelToOpen.transform.DOScale(panelOriginalScale, animationDuration).SetEase(Ease.OutBack);
                });
        }
        else
        {
            Debug.Log("Pas assez de playerPower pour effectuer cette action.");
        }
    }

    // Mettre à jour l'opacité et l'interactivité du bouton en fonction de playerPower
    void UpdateButtonInteractivity()
    {
        if (powerBarManager.playerPower < prix)
        {
            // Rendre le bouton inactif avec une opacité réduite
            switchButton.interactable = false;
            switchButton.image.color = new Color(switchButton.image.color.r, switchButton.image.color.g, switchButton.image.color.b, 0.4f);
        }
        else
        {
            // Rendre le bouton actif avec une opacité pleine
            switchButton.interactable = true;
            switchButton.image.color = new Color(switchButton.image.color.r, switchButton.image.color.g, switchButton.image.color.b, 1f);
        }
    }
}
