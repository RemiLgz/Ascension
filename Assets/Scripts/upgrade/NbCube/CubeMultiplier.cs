using UnityEngine;
using TMPro;

public class CubeMultiplier : MonoBehaviour
{
    public PowerBarManager powerBarManager;       // Référence au PowerBarManager
    public int multiplier = 2;                    // Multiplicateur de cubes par clic
    public int prix = 50;                         // Prix initial de l'achat
    public int prixAugmentation = 20;             // Augmentation du prix après chaque achat
    public TextMeshProUGUI prixText;              // Texte pour afficher le prix
    public TextMeshProUGUI niveauText;            // Texte pour afficher le niveau d'achat

    private int niveau = 1;                       // Niveau d'achat actuel

    private void Start()
    {
        if (powerBarManager == null)
        {
            Debug.LogError("PowerBarManager n'est pas assigné dans CubeMultiplier.");
        }

        // Mettre à jour les textes au démarrage
        UpdateUI();
    }

    // Méthode appelée lors du clic sur le bouton
    public void AcheterMultiplicateur()
    {
        if (powerBarManager != null)
        {
            // Vérifie si le joueur a assez de playerPower pour acheter
            if (powerBarManager.playerPower >= prix)
            {
                // Retire le playerPower nécessaire pour l'achat
                powerBarManager.playerPower -= prix;

                // Multiplie le nombre de cubes par clic
                powerBarManager.cubesPerClick *= multiplier;

                // Augmente le prix et le niveau pour le prochain achat
                prix += prixAugmentation;
                niveau++;

                // Mettre à jour l'UI
                UpdateUI();

                Debug.Log("Multiplicateur acheté ! Nouveau cubes par clic : " + powerBarManager.cubesPerClick);
                Debug.Log("Power restant : " + powerBarManager.playerPower);
            }
            else
            {
                Debug.Log("Pas assez de power pour acheter le multiplicateur !");
            }
        }
    }

    private void UpdateUI()
    {
        if (prixText != null)
        {
            prixText.text = "" + prix;
        }

        if (niveauText != null)
        {
            niveauText.text = "Niv. " + niveau;
        }
    }
}
