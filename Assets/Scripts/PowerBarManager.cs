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

    private float currentPower = 0f;
    private int playerPower = 0;
    private float maxPower;

    void Start()
    {
        UpdateMaxPower();
    }

    void Update()
    {
        // Vérifie si le clic est effectué sans élément UI actif sous le pointeur
        if (Input.GetMouseButtonDown(0) && !IsPointerOverActiveUI())
        {
            for (int i = 0; i < cubesPerClick; i++)
            {
                CreateAndAnimateObject();
            }

            CreateWaveEffect();
        }
    }

    private bool IsPointerOverActiveUI()
    {
        // Vérifie si la souris est au-dessus d'un élément UI et si cet élément est activé dans la hiérarchie
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Vérifie chaque élément sous le pointeur
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.activeInHierarchy) // Ignore les éléments désactivés
            {
                return true; // Retourne `true` si un élément UI activé est détecté
            }
        }
        return false; // Retourne `false` si aucun élément UI activé n'est détecté
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

        if (currentPower >= maxPower)
        {
            currentPower = 0f;
            playerPower++;
            UpdatePowerText();
            UpdateMaxPower();
            CreateFixedLevelUpWave();
        }

        AnimatePowerBar();
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
        powerText.transform.DOKill();
        powerText.transform.DOPunchScale(Vector2.one * bounceScale, bounceDuration);
    }
}
