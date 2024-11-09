using UnityEngine;
using UnityEngine.UI;

public class EnemyDestroyer : MonoBehaviour
{
    public GameObject destroyPoint;
    public string enemyTag = "Enemy";
    public GameObject[] debrisPrefabs;
    public float debrisForce = 500f;
    public int debrisCount = 10;
    public Camera mainCamera;
    public Canvas flashCanvas;
    public Image slashImage;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.3f;
    public float debrisLifetime = 2.0f;
    public float fillSpeed = 8f;
    public float flashDuration = 0.1f;
    public float triggerPercentage = 0.7f;

    private bool isFilling = false;
    private GameObject currentEnemy;

    public PowerBarManager powerBarManager;
    public int powerIncrease = 10;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (!isFilling && enemy.transform.position.x <= destroyPoint.transform.position.x)
            {
                currentEnemy = enemy;
                isFilling = true;
            }
        }

        if (isFilling)
        {
            FillSlashImage();
        }
    }

    void FillSlashImage()
    {
        slashImage.fillAmount = Mathf.Lerp(slashImage.fillAmount, 1f, Time.deltaTime * fillSpeed);

        if (slashImage.fillAmount >= triggerPercentage)
        {
            slashImage.fillAmount = 0;
            isFilling = false;
            TriggerEffects();
        }
    }

    void TriggerEffects()
    {
        if (currentEnemy != null)
        {
            foreach (GameObject debrisPrefab in debrisPrefabs)
            {
                for (int i = 0; i < debrisCount; i++)
                {
                    GameObject debris = Instantiate(debrisPrefab, currentEnemy.transform.position, Random.rotation);
                    Rigidbody rb = debris.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 randomDirection = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f)).normalized;
                        rb.AddForce(randomDirection * debrisForce, ForceMode.Impulse);
                    }
                    Destroy(debris, debrisLifetime);
                }
            }

            if (mainCamera != null)
            {
                StartCoroutine(CameraShake());
            }

            StartCoroutine(ScreenFlash());
            Destroy(currentEnemy);
        }
    }

    System.Collections.IEnumerator CameraShake()
    {
        Vector3 originalPosition = mainCamera.transform.position;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            mainCamera.transform.position = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPosition;
    }

    System.Collections.IEnumerator ScreenFlash()
    {
        GameObject flash = new GameObject("Flash");
        flash.transform.SetParent(flashCanvas.transform);
        flash.transform.localPosition = Vector3.zero;

        UnityEngine.UI.Image image = flash.AddComponent<UnityEngine.UI.Image>();
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        image.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(flashDuration);

        // Ajout à currentPower pendant le flash
        if (powerBarManager != null)
        {
            powerBarManager.currentPower += powerIncrease;
            Debug.Log($"currentPower après flash blanc: {powerBarManager.currentPower}");
        }
        else
        {
            Debug.LogWarning("PowerBarManager n'est pas assigné dans EnemyDestroyer.");
        }

        Destroy(flash);
    }
}
