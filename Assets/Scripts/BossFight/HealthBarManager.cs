using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Assurez-vous d'ajouter cette ligne pour utiliser DOTween

public class HealthBarManager : MonoBehaviour
{
    public Image healthBarImage; // L'image repr�sentant la barre de vie
    public ImageSwitcher imageSwitcher; // R�f�rence au script ImageSwitcher
    private float maxHealth; // La valeur initiale de bossHealth
    private float targetHealthPercentage; // Le pourcentage de sant� cible

    // La dur�e de l'animation de la barre de vie
    public float smoothDuration = 0.5f;

    void Start()
    {
        if (imageSwitcher == null || healthBarImage == null)
        {
            Debug.LogError("R�f�rences manquantes ! Veuillez assigner ImageSwitcher et HealthBarImage.");
            return;
        }

        // R�cup�rer la sant� du boss au d�but de la sc�ne
        maxHealth = imageSwitcher.bossHealth;

        // Initialiser la barre de vie avec la sant� maximale du boss
        UpdateHealthBar();
    }

    void Update()
    {
        // Mettre � jour la barre de vie � chaque frame
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (imageSwitcher != null && healthBarImage != null)
        {
            // Calculer la proportion de la barre de vie
            float currentHealth = imageSwitcher.bossHealth;
            float healthPercentage = currentHealth / maxHealth;

            // V�rifier si la valeur cible a chang�, et si oui, animer la barre
            if (Mathf.Abs(targetHealthPercentage - healthPercentage) > Mathf.Epsilon)
            {
                // Animer la barre de vie de mani�re fluide
                healthBarImage.DOFillAmount(healthPercentage, smoothDuration);
                targetHealthPercentage = healthPercentage; // Mettre � jour la cible de sant�
            }
        }
    }
}
