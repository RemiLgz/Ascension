using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PowerBarManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image powerBar;
    public TextMeshProUGUI powerText;

    [Header("Object Prefab")]
    public GameObject objectPrefab;
    public GameObject wavePrefab;

    [Header("Power Settings")]
    public float powerIncrement = 5f;
    public float baseMaxPower = 100f;
    public float difficultyMultiplier = 20f; // Ajout du multiplicateur de difficulté

    [Header("Spawn Settings")]
    public int cubesPerClick = 1;

    [Header("Animation Settings")]
    public float fillDuration = 0.5f;
    public float bounceDuration = 0.2f;
    public float bounceScale = 1.2f;

    [Header("Movement Settings")]
    public Vector3 targetPosition = new Vector3(0f, 0f, 0f);
    public float speed = 1f;
    public float destroyOffset = 0.5f;
    public float randomDistance = 2f;
    public float delayBeforeMoving = 1f;

    [Header("Wave Settings")]
    public float waveScale = 2f;
    public float waveFadeDuration = 0.5f;

    [Header("Level Up Animation Settings")]
    public GameObject levelUpWavePrefab;
    public float levelUpWaveScale = 3f;
    public float levelUpWaveDuration = 0.5f;
    public float levelUpWaveFadeDuration = 0.5f;

    [Header("Fixed Wave Position (for Level Up)")]
    public Vector3 fixedWavePosition = new Vector3(0f, 0f, 0f);

    // Valeurs
    public float currentPower = 0f;
    public int playerPower = 0;
    public float maxPower; // La maxPower dépendra de playerPower et de difficultyMultiplier

    private int displayedPlayerPower;

    void Start()
    {
        // Initialisation maxPower dès le départ en fonction de playerPower et du multiplicateur
        UpdateMaxPower();
        UpdatePowerText();
    }

    void Update()
    {
        // Toujours recalculer maxPower en temps réel en fonction de playerPower
        UpdateMaxPower();

        // Vérifier les clics pour créer et animer les objets
        if (Input.GetMouseButtonDown(0) && !IsPointerOverActiveUI())
        {
            for (int i = 0; i < cubesPerClick; i++)
            {
                CreateAndAnimateObject();
            }

            // Créer l'effet de vague à chaque clic
            CreateWaveEffect();
        }

        // Remplir la barre de puissance en fonction de currentPower et maxPower
        AnimatePowerBar();

        // Si currentPower dépasse maxPower, augmenter playerPower
        if (currentPower >= maxPower)
        {
            float excessPower = currentPower - maxPower;
            playerPower++; // Augmenter le niveau du joueur
            currentPower = excessPower; // Réinitialiser currentPower avec l'excédent
            // Créer l'effet visuel de montée de niveau
            CreateFixedLevelUpWave();

            // Mettre à jour l'affichage du texte
            UpdatePowerText();
        }

        // Met à jour le texte si playerPower change
        if (playerPower != displayedPlayerPower)
        {
            UpdatePowerText();
        }
    }

    private bool IsPointerOverActiveUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    void CreateAndAnimateObject()
    {
        // Créer un objet et l'animer
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        GameObject obj = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = obj.AddComponent<Rigidbody>();
        Vector3 randomForce = new Vector3(Random.Range(-5f, 5f), Random.Range(2f, 5f), Random.Range(-5f, 5f));
        rb.AddForce(randomForce, ForceMode.Impulse);

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 randomTarget = spawnPosition + randomDirection * randomDistance;

        obj.transform.DOMove(randomTarget, delayBeforeMoving).OnComplete(() => {
            float moveDuration = Vector3.Distance(randomTarget, targetPosition) / speed;
            obj.transform.DOMove(targetPosition, moveDuration).OnComplete(() => {
                if (Vector3.Distance(obj.transform.position, targetPosition) < destroyOffset)
                {
                    Destroy(obj);
                    IncreasePower();
                }
            }).SetEase(Ease.Linear);
        });

        StartCoroutine(RotateObject(obj));
    }

    private System.Collections.IEnumerator RotateObject(GameObject obj)
    {
        while (obj != null)
        {
            obj.transform.Rotate(Random.Range(-1f, 1f) * 10f, Random.Range(-1f, 1f) * 10f, Random.Range(-1f, 1f) * 10f);
            yield return null;
        }
    }

    void CreateWaveEffect()
    {
        // Créer l'effet de vague à la position du clic
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        spawnPosition.z = 0f;

        GameObject wave = Instantiate(wavePrefab, spawnPosition, Quaternion.identity);
        wave.transform.localScale = Vector3.zero;

        wave.transform.DOScale(Vector3.one * waveScale, 0.5f).OnComplete(() => {
            SpriteRenderer spriteRenderer = wave.GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(0f, waveFadeDuration).OnComplete(() => {
                Destroy(wave);
            });
        });
    }

    void IncreasePower()
    {
        // Ajouter de la puissance actuelle à chaque clic
        currentPower += powerIncrement;
    }

    void CreateFixedLevelUpWave()
    {
        // Créer l'onde de niveau
        Vector3 wavePosition = fixedWavePosition;

        GameObject levelUpWave = Instantiate(levelUpWavePrefab, wavePosition, Quaternion.identity);
        levelUpWave.transform.localScale = Vector3.zero;

        levelUpWave.transform.DOScale(Vector3.one * levelUpWaveScale, levelUpWaveDuration).OnComplete(() => {
            SpriteRenderer spriteRenderer = levelUpWave.GetComponent<SpriteRenderer>();
            spriteRenderer.DOFade(0f, levelUpWaveFadeDuration).OnComplete(() => {
                Destroy(levelUpWave);
            });
        });
    }

    // Update maxPower en fonction de playerPower à chaque frame
    void UpdateMaxPower()
    {
        // Calcul de maxPower en fonction de playerPower et du multiplicateur de difficulté
        maxPower = baseMaxPower + (playerPower * difficultyMultiplier);
    }

    private Tween _bounceTween;
    private Tween _fillTween;

    void AnimatePowerBar()
    {
        // Remplir la barre de puissance en temps réel
        float normalizedPower = currentPower / maxPower;
        _fillTween?.Kill();
        _fillTween = powerBar.DOFillAmount(normalizedPower, fillDuration);

        _bounceTween?.Complete();
        _bounceTween = powerBar.transform.DOPunchScale(Vector2.one * bounceScale, bounceDuration);
    }

    void UpdatePowerText()
    {
        // Met à jour le texte affichant la puissance du joueur
        powerText.text = playerPower.ToString();
        displayedPlayerPower = playerPower;

        // Animation de mise à jour du texte
        powerText.transform.DOKill();
    }

    // Méthode pour gérer un achat d'objet
    public void PurchaseItem(float playerPowerChange)
    {
        // Modifier playerPower directement
        playerPower += Mathf.FloorToInt(playerPowerChange);

        // Mettre à jour maxPower immédiatement en fonction du nouveau playerPower et du multiplicateur
        UpdateMaxPower();

        // Mettre à jour l'affichage de la puissance
        UpdatePowerText();
    }

    // Méthode pour retirer de la puissance du joueur
    public void RemovePower(int amount)
    {
        // Réduire le playerPower et mettre à jour maxPower immédiatement
        playerPower = Mathf.Max(0, playerPower - amount);
        UpdateMaxPower();

        // Mettre à jour l'affichage de la puissance
        UpdatePowerText();
    }
}
