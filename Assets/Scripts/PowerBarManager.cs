using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class PowerBarManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image powerBar;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI newText; // Nouveau texte ajouté

    [Header("Object Prefab")]
    public GameObject objectPrefab;
    public GameObject wavePrefab;

    [Header("Power Settings")]
    public float powerIncrement = 5f;
    public float baseMaxPower = 100f;
    public float difficultyMultiplier = 20f;

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

    // Références aux scripts EnemySpawner et EnemyDestroyer
    public EnemySpawner enemySpawner;
    public EnemyDestroyer enemyDestroyer;

    // Valeurs
    public float currentPower = 0f;
    public int playerPower = 0;
    public float maxPower;

    private int displayedPlayerPower;
    private float displayedMultiplierValue = 0f;
    private List<float> tempAddedValues = new List<float>(); // Liste pour gérer les ajouts temporaires

    void Start()
    {
        UpdateMaxPower();
        UpdatePowerText();
        UpdateMultiplierValue();
    }

    void Update()
    {
        // Toujours mettre à jour maxPower en temps réel
        UpdateMaxPower();

        // Calculer le produit actuel de powerIncrease / spawnInterval
        UpdateMultiplierValue();

        // Vérifier les clics pour générer des objets
        if (Input.GetMouseButtonDown(0) && !IsPointerOverActiveUI())
        {
            for (int i = 0; i < cubesPerClick; i++)
            {
                CreateAndAnimateObject();
            }
            CreateWaveEffect();

            // Ajouter la valeur de (powerIncrement * cubesPerClick) au texte
            AddTempValueToText((powerIncrement * cubesPerClick));

            // Retirer la valeur ajoutée après 1 seconde
            StartCoroutine(RemoveTempValueAfterDelay(1f, (powerIncrement * cubesPerClick)));
        }

        AnimatePowerBar();

        if (currentPower >= maxPower)
        {
            float excessPower = currentPower - maxPower;
            playerPower++;
            currentPower = excessPower;
            CreateFixedLevelUpWave();
            UpdatePowerText();
        }

        if (playerPower != displayedPlayerPower)
        {
            UpdatePowerText();
        }
    }

    private void UpdateMultiplierValue()
    {
        // Vérifie si les objets EnemySpawner et EnemyDestroyer existent et récupère spawnInterval et powerIncrease
        if (enemySpawner != null && enemyDestroyer != null)
        {
            // Calculer powerIncrease / spawnInterval
            float currentMultiplierValue = enemyDestroyer.powerIncrease / enemySpawner.spawnInterval;

            // Si la valeur a changé, on met à jour newText
            if (currentMultiplierValue != displayedMultiplierValue)
            {
                displayedMultiplierValue = currentMultiplierValue;
                newText.text = Mathf.FloorToInt(displayedMultiplierValue).ToString() + " par sec"; // Affichage sans décimales et avec "/sec"
            }
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
        currentPower += powerIncrement;
    }

    void CreateFixedLevelUpWave()
    {
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

    void UpdateMaxPower()
    {
        maxPower = baseMaxPower + (playerPower * difficultyMultiplier);
    }

    private Tween _bounceTween;
    private Tween _fillTween;

    void AnimatePowerBar()
    {
        float normalizedPower = currentPower / maxPower;
        _fillTween?.Kill();
        _fillTween = powerBar.DOFillAmount(normalizedPower, fillDuration);

        _bounceTween?.Complete();
        _bounceTween = powerBar.transform.DOPunchScale(Vector2.one * bounceScale, bounceDuration);
    }

    void UpdatePowerText()
    {
        powerText.text = playerPower.ToString();
        displayedPlayerPower = playerPower;
        powerText.transform.DOKill();
    }

    void AddTempValueToText(float value)
    {
        // Ajouter la valeur temporaire au texte
        tempAddedValues.Add(value);
        float totalTempValue = 0f;

        // Calculer la somme des valeurs temporaires
        foreach (float tempValue in tempAddedValues)
        {
            totalTempValue += tempValue;
        }

        // Mettre à jour le texte
        newText.text = Mathf.FloorToInt(displayedMultiplierValue + totalTempValue).ToString() + " par sec";
    }

    private System.Collections.IEnumerator RemoveTempValueAfterDelay(float delay, float valueToRemove)
    {
        // Attendre le délai et retirer la valeur spécifique ajoutée
        yield return new WaitForSeconds(delay);
        tempAddedValues.Remove(valueToRemove); // Retirer uniquement la valeur ajoutée récemment

        // Mettre à jour le texte après avoir retiré la valeur
        float totalTempValue = 0f;
        foreach (float tempValue in tempAddedValues)
        {
            totalTempValue += tempValue;
        }

        newText.text = Mathf.FloorToInt(displayedMultiplierValue + totalTempValue).ToString() + " par sec";
    }

    public void PurchaseItem(float playerPowerChange)
    {
        playerPower += Mathf.FloorToInt(playerPowerChange);
        UpdateMaxPower();
        UpdatePowerText();
    }

    public void RemovePower(int amount)
    {
        playerPower = Mathf.Max(0, playerPower - amount);
        UpdateMaxPower();
        UpdatePowerText();
    }
}
