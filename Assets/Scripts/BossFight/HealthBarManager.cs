using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Assurez-vous d'ajouter cette ligne pour utiliser DOTween

public class HealthBarManager : MonoBehaviour
{
    public Image healthBarImage; // L'image représentant la barre de vie
    public ImageSwitcher imageSwitcher; // Référence au script ImageSwitcher
    private float maxHealth; // La valeur initiale de bossHealth
    private float targetHealthPercentage; // Le pourcentage de santé cible

    // La durée de l'animation de la barre de vie
    public float smoothDuration = 0.5f;

    void Start()
    {
        if (imageSwitcher == null || healthBarImage == null)
        {
            Debug.LogError("Références manquantes ! Veuillez assigner ImageSwitcher et HealthBarImage.");
            return;
        }

        // Récupérer la santé du boss au début de la scène
        maxHealth = imageSwitcher.bossHealth;

        // Initialiser la barre de vie avec la santé maximale du boss
        UpdateHealthBar();
    }

    void Update()
    {
        // Mettre à jour la barre de vie à chaque frame
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (imageSwitcher != null && healthBarImage != null)
        {
            // Calculer la proportion de la barre de vie
            float currentHealth = imageSwitcher.bossHealth;
            float healthPercentage = currentHealth / maxHealth;

            // Vérifier si la valeur cible a changé, et si oui, animer la barre
            if (Mathf.Abs(targetHealthPercentage - healthPercentage) > Mathf.Epsilon)
            {
                // Animer la barre de vie de manière fluide
                healthBarImage.DOFillAmount(healthPercentage, smoothDuration);
                targetHealthPercentage = healthPercentage; // Mettre à jour la cible de santé
            }
        }
    }
}
