using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerIncreaseUpgrader : MonoBehaviour
{
    public PowerBarManager powerBarManager;          // Référence au PowerBarManager
    public EnemyDestroyer enemyDestroyer;            // Référence au script EnemyDestroyer
    public Button acheterButton;                     // Bouton d'achat
    public float powerIncreaseMultiplier = 1.2f;     // Facteur d'augmentation pour powerIncrease (20%)
    public float prixMultiplicateur = 1.2f;          // Facteur d'augmentation du prix (20%)
    public int prixBase = 50;                        // Prix de base de l'achat
    public TextMeshProUGUI prixText;                 // Texte pour afficher le prix
    public TextMeshProUGUI niveauText;               // Texte pour afficher le niveau d'achat

    private int niveau = 1;                          // Niveau d'achat actuel
    private int prix;                                // Prix actuel, qui augmente après chaque achat

    private void Start()
    {
        if (powerBarManager == null)
        {
            Debug.LogError("PowerBarManager n'est pas assigné dans PowerIncreaseUpgrader.");
        }

        if (enemyDestroyer == null)
        {
            Debug.LogError("EnemyDestroyer n'est pas assigné dans PowerIncreaseUpgrader.");
        }

        if (acheterButton == null)
        {
            Debug.LogError("Le bouton d'achat n'est pas assigné dans PowerIncreaseUpgrader.");
        }

        prix = prixBase; // Initialise le prix avec le prix de base

        // Mettre à jour l'UI au démarrage
        UpdateUI();
    }

    private void Update()
    {
        // Mettre à jour l'état du bouton d'achat à chaque frame
        UpdateButtonState();
    }

    // Méthode appelée lors du clic sur le bouton
    public void AcheterAmeliorationPower()
    {
        if (powerBarManager != null && enemyDestroyer != null)
        {
            // Vérifie si le joueur a assez de playerPower pour acheter
            if (powerBarManager.playerPower >= prix)
            {
                // Retire le playerPower nécessaire pour l'achat
                powerBarManager.playerPower -= prix;

                // Augmente de 20% la valeur de powerIncrease et convertit le résultat en int
                enemyDestroyer.powerIncrease = (int)(enemyDestroyer.powerIncrease * powerIncreaseMultiplier);

                // Augmente le prix pour le prochain achat avec un pourcentage appliqué au prix de base
                niveau++;
                prix = Mathf.CeilToInt(prixBase * Mathf.Pow(prixMultiplicateur, niveau - 1));

                // Mettre à jour l'UI
                UpdateUI();

                Debug.Log("Amélioration de powerIncrease achetée ! Nouveau powerIncrease : " + enemyDestroyer.powerIncrease);
                Debug.Log("Power restant : " + powerBarManager.playerPower);
            }
            else
            {
                Debug.Log("Pas assez de power pour acheter l'amélioration de powerIncrease !");
            }
        }
    }

    private void UpdateUI()
    {
        if (prixText != null)
        {
            prixText.text = prix.ToString();
        }

        if (niveauText != null)
        {
            niveauText.text = "Niv. " + niveau;
        }
    }

    private void UpdateButtonState()
    {
        if (acheterButton != null && powerBarManager != null)
        {
            // Change l'opacité du bouton en fonction des ressources disponibles
            Color buttonColor = acheterButton.image.color;
            if (powerBarManager.playerPower >= prix)
            {
                // Assez de power : opacité à 100%
                buttonColor.a = 1f;
                acheterButton.interactable = true;
            }
            else
            {
                // Pas assez de power : opacité à 40%
                buttonColor.a = 0.4f;
                acheterButton.interactable = false;
            }
            acheterButton.image.color = buttonColor;
        }
    }
}
