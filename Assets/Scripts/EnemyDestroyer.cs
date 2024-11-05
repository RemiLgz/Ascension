using UnityEngine;

public class EnemyDestroyer : MonoBehaviour
{
    public GameObject destroyPoint;
    public string enemyTag = "Enemy";
    public GameObject[] debrisPrefabs;
    public float debrisForce = 500f;
    public int debrisCount = 10;
    public Camera mainCamera;
    public Canvas flashCanvas; // Référence au Canvas pour le flash
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.3f;
    public float flashDuration = 0.1f;
    public float debrisLifetime = 2.0f;
    private bool isFlashing = false;

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemy in enemies)
        {
            if (enemy.transform.position.x <= destroyPoint.transform.position.x)
            {
                ExplodeEnemy(enemy);
            }
        }
    }

    void ExplodeEnemy(GameObject enemy)
    {
        foreach (GameObject debrisPrefab in debrisPrefabs)
        {
            for (int i = 0; i < debrisCount; i++)
            {
                GameObject debris = Instantiate(debrisPrefab, enemy.transform.position, Random.rotation);
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
            if (!isFlashing) StartCoroutine(ScreenFlash());
        }

        Destroy(enemy);
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
        isFlashing = true;

        GameObject flash = new GameObject("Flash");
        flash.transform.SetParent(flashCanvas.transform);
        flash.transform.localPosition = Vector3.zero;

        Canvas canvas = flashCanvas; // Utilise le Canvas choisi
        GameObject flashImage = new GameObject("FlashImage");
        flashImage.transform.SetParent(flash.transform);

        UnityEngine.UI.Image image = flashImage.AddComponent<UnityEngine.UI.Image>();
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;

        image.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(flashDuration);
        Destroy(flash);
        isFlashing = false;
    }
}
