using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpawnIntervalReducer : MonoBehaviour
{
    public PowerBarManager powerBarManager;          // R�f�rence au PowerBarManager
    public EnemySpawner enemySpawner;                // R�f�rence au script EnemySpawner
    public Button acheterButton;                     // Bouton d'achat
    public float spawnIntervalReductionMultiplier = 0.8f; // Facteur de r�duction pour spawnInterval (par exemple, 20% de r�duction)
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
            Debug.LogError("PowerBarManager n'est pas assign� dans SpawnIntervalReducer.");
        }

        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner n'est pas assign� dans SpawnIntervalReducer.");
        }

        if (acheterButton == null)
        {
            Debug.LogError("Le bouton d'achat n'est pas assign� dans SpawnIntervalReducer.");
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
    public void AcheterReductionSpawnInterval()
    {
        if (powerBarManager != null && enemySpawner != null)
        {
            // V�rifie si le joueur a assez de playerPower pour acheter
            if (powerBarManager.playerPower >= prix)
            {
                // Retire le playerPower n�cessaire pour l'achat
                powerBarManager.playerPower -= prix;

                // R�duit de 20% la valeur de spawnInterval
                enemySpawner.spawnInterval *= spawnIntervalReductionMultiplier;

                // Augmente le prix pour le prochain achat avec un pourcentage appliqu� au prix de base
                niveau++;
                prix = Mathf.CeilToInt(prixBase * Mathf.Pow(prixMultiplicateur, niveau - 1));

                // Mettre � jour l'UI
                UpdateUI();

                Debug.Log("R�duction du spawnInterval achet�e ! Nouveau spawnInterval : " + enemySpawner.spawnInterval);
                Debug.Log("Power restant : " + powerBarManager.playerPower);
            }
            else
            {
                Debug.Log("Pas assez de power pour acheter la r�duction de spawnInterval !");
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
