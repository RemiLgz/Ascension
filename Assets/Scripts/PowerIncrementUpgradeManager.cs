using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerIncrementUpgradeManager : MonoBehaviour
{
    public PowerBarManager powerBarManager;      // Référence au script PowerBarManager
    public Button upgradeButton;                 // Le bouton pour améliorer la compétence
    public TextMeshProUGUI prixText;             // Texte pour afficher le prix de l'amélioration
    public TextMeshProUGUI niveauText;           // Texte pour afficher le niveau de l'amélioration

    public int prix = 50;                        // Coût initial de l'amélioration
    private int niveauAmelioration = 1;          // Niveau initial de l'amélioration

    public float multiplierPowerIncrement = 1.5f; // Multiplicateur pour l'augmentation de powerIncrement
    public float multiplierPrixBase = 1.5f;       // Multiplicateur de base pour l'augmentation du prix
    public float prixNiveauIncrement = 0.1f;      // Incrément par niveau pour le calcul du prix

    void Start()
    {
        // Relier le bouton à la fonction UpgradeSkill
        upgradeButton.onClick.AddListener(UpgradeSkill);

        // Afficher le prix et le niveau initiaux dans les textes
        UpdatePrixText();
        UpdateNiveauText();
        UpdateButtonInteractivity(); // Vérifie l'état du bouton au lancement
    }

    void Update()
    {
        // Mettre à jour l'apparence du bouton à chaque frame
        UpdateButtonInteractivity();
    }

    // Méthode pour améliorer la compétence
    void UpgradeSkill()
    {
        // Vérifier si le joueur a suffisamment de "playerPower" pour payer le prix
        if (powerBarManager.playerPower >= prix)
        {
            // Soustraire le prix à playerPower
            powerBarManager.playerPower -= prix;

            // Augmenter powerIncrement de façon exponentielle mais contrôlée
            powerBarManager.powerIncrement = Mathf.RoundToInt(powerBarManager.powerIncrement * multiplierPowerIncrement);

            // Calculer le nouveau prix pour la prochaine amélioration
            prix = Mathf.RoundToInt(prix * (multiplierPrixBase + (prixNiveauIncrement * niveauAmelioration)));

            // Incrémenter le niveau d'amélioration
            niveauAmelioration++;

            // Mettre à jour l'affichage du prix et du niveau
            UpdatePrixText();
            UpdateNiveauText();
        }
    }

    // Mettre à jour l'affichage du prix dans le TextMeshPro
    void UpdatePrixText()
    {
        prixText.text = prix.ToString();
    }

    // Mettre à jour l'affichage du niveau dans le TextMeshPro
    void UpdateNiveauText()
    {
        niveauText.text = "Niv." + niveauAmelioration.ToString();
    }

    // Ajuster la transparence et l'interactivité du bouton en fonction de la capacité d'achat du joueur
    void UpdateButtonInteractivity()
    {
        // Vérifie si le joueur a assez de power pour interagir avec le bouton
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
