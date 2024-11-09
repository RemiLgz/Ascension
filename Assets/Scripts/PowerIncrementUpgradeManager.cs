using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerIncrementUpgradeManager : MonoBehaviour
{
    public PowerBarManager powerBarManager;      // R�f�rence au script PowerBarManager
    public Button upgradeButton;                 // Le bouton pour am�liorer la comp�tence
    public TextMeshProUGUI prixText;             // Texte pour afficher le prix de l'am�lioration
    public TextMeshProUGUI niveauText;           // Texte pour afficher le niveau de l'am�lioration

    public int prix = 50;                        // Co�t initial de l'am�lioration
    private int niveauAmelioration = 1;          // Niveau initial de l'am�lioration

    public float multiplierPowerIncrement = 1.5f; // Multiplicateur pour l'augmentation de powerIncrement
    public float multiplierPrixBase = 1.5f;       // Multiplicateur de base pour l'augmentation du prix
    public float prixNiveauIncrement = 0.1f;      // Incr�ment par niveau pour le calcul du prix

    void Start()
    {
        // Relier le bouton � la fonction UpgradeSkill
        upgradeButton.onClick.AddListener(UpgradeSkill);

        // Afficher le prix et le niveau initiaux dans les textes
        UpdatePrixText();
        UpdateNiveauText();
        UpdateButtonInteractivity(); // V�rifie l'�tat du bouton au lancement
    }

    void Update()
    {
        // Mettre � jour l'apparence du bouton � chaque frame
        UpdateButtonInteractivity();
    }

    // M�thode pour am�liorer la comp�tence
    void UpgradeSkill()
    {
        // V�rifier si le joueur a suffisamment de "playerPower" pour payer le prix
        if (powerBarManager.playerPower >= prix)
        {
            // Soustraire le prix � playerPower
            powerBarManager.playerPower -= prix;

            // Augmenter powerIncrement de fa�on exponentielle mais contr�l�e
            powerBarManager.powerIncrement = Mathf.RoundToInt(powerBarManager.powerIncrement * multiplierPowerIncrement);

            // Calculer le nouveau prix pour la prochaine am�lioration
            prix = Mathf.RoundToInt(prix * (multiplierPrixBase + (prixNiveauIncrement * niveauAmelioration)));

            // Incr�menter le niveau d'am�lioration
            niveauAmelioration++;

            // Mettre � jour l'affichage du prix et du niveau
            UpdatePrixText();
            UpdateNiveauText();
        }
    }

    // Mettre � jour l'affichage du prix dans le TextMeshPro
    void UpdatePrixText()
    {
        prixText.text = prix.ToString();
    }

    // Mettre � jour l'affichage du niveau dans le TextMeshPro
    void UpdateNiveauText()
    {
        niveauText.text = "Niv." + niveauAmelioration.ToString();
    }

    // Ajuster la transparence et l'interactivit� du bouton en fonction de la capacit� d'achat du joueur
    void UpdateButtonInteractivity()
    {
        // V�rifie si le joueur a assez de power pour interagir avec le bouton
        if (powerBarManager.playerPower < prix)
        {
            upgradeButton.interactable = false;
            upgradeButton.image.color = new Color(upgradeButton.image.color.r, upgradeButton.image.color.g, upgradeButton.image.color.b, 0.4f);
        }
        else
        {
            upgradeButton.interactable = true;
            upgradeButton.image.color = new Color(upgradeButton.image.color.r, upgradeButton.image.color.g, upgradeButton.image.color.b, 1f);
        }
    }
}
