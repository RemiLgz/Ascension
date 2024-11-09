using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PowerIncreaseUpgrader : MonoBehaviour
{
    public PowerBarManager powerBarManager;          // R�f�rence au PowerBarManager
    public EnemyDestroyer enemyDestroyer;            // R�f�rence au script EnemyDestroyer
    public Button acheterButton;                     // Bouton d'achat
    public float powerIncreaseMultiplier = 1.2f;     // Facteur d'augmentation pour powerIncrease (20%)
    public float prixMultiplicateur = 1.2f;          // Facteur d'augmentation du prix (20%)
    public int prixBase = 50;                        // Prix de base de l'achat
    public TextMeshProUGUI prixText;                 // Texte pour afficher le prix
    public TextMeshProUGUI niveauText;               // Texte pour afficher le niveau d'achat

    private int niveau = 1;                          // Niveau d'achat actuel
    private int prix;                                // Prix actuel, qui augmente apr�s chaque achat

    private void Start()
    {
        if (powerBarManager == null)
        {
            Debug.LogError("PowerBarManager n'est pas assign� dans PowerIncreaseUpgrader.");
        }

        if (enemyDestroyer == null)
        {
            Debug.LogError("EnemyDestroyer n'est pas assign� dans PowerIncreaseUpgrader.");
        }

        if (acheterButton == null)
        {
            Debug.LogError("Le bouton d'achat n'est pas assign� dans PowerIncreaseUpgrader.");
        }

        prix = prixBase; // Initialise le prix avec le prix de base

        // Mettre � jour l'UI au d�marrage
        UpdateUI();
    }

    private void Update()
    {
        // Mettre � jour l'�tat du bouton d'achat � chaque frame
        UpdateButtonState();
    }

    // M�thode appel�e lors du clic sur le bouton
    public void AcheterAmeliorationPower()
    {
        if (powerBarManager != null && enemyDestroyer != null)
        {
            // V�rifie si le joueur a assez de playerPower pour acheter
            if (powerBarManager.playerPower >= prix)
            {
                // Retire le playerPower n�cessaire pour l'achat
                powerBarManager.playerPower -= prix;

                // Augmente de 20% la valeur de powerIncrease et convertit le r�sultat en int
                enemyDestroyer.powerIncrease = (int)(enemyDestroyer.powerIncrease * powerIncreaseMultiplier);

                // Augmente le prix pour le prochain achat avec un pourcentage appliqu� au prix de base
                niveau++;
                prix = Mathf.CeilToInt(prixBase * Mathf.Pow(prixMultiplicateur, niveau - 1));

                // Mettre � jour l'UI
                UpdateUI();

                Debug.Log("Am�lioration de powerIncrease achet�e ! Nouveau powerIncrease : " + enemyDestroyer.powerIncrease);
                Debug.Log("Power restant : " + powerBarManager.playerPower);
            }
            else
            {
                Debug.Log("Pas assez de power pour acheter l'am�lioration de powerIncrease !");
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
            // Change l'opacit� du bouton en fonction des ressources disponibles
            Color buttonColor = acheterButton.image.color;
            if (powerBarManager.playerPower >= prix)
            {
                // Assez de power : opacit� � 100%
                buttonColor.a = 1f;
                acheterButton.interactable = true;
            }
            else
            {
                // Pas assez de power : opacit� � 40%
                buttonColor.a = 0.4f;
                acheterButton.interactable = false;
            }
            acheterButton.image.color = buttonColor;
        }
    }
}
