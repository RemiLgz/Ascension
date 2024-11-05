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
    public Canvas flashCanvas; // Assigner le FlashCanvas ici
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.3f;
    public float flashDuration = 0.1f;
    public float debrisLifetime = 2.0f;
    private bool isFlashing = false;

    private Image flashImage; // Référence à l'image blanche pour le flash

    void Start()
    {
        // Obtenir la référence à l'image du flash
        flashImage = flashCanvas.GetComponentInChildren<Image>();
        if (flashImage == null)
        {
            Debug.LogError("Aucune Image trouvée dans FlashCanvas pour le flash blanc.");
        }
    }

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

        if (flashImage == null) yield break; // S'assurer que flashImage existe

        flashImage.color = new Color(1, 1, 1, 1); // Rend l'image complètement blanche

        yield return new WaitForSeconds(flashDuration);

        // Fondu de l'image pour la faire disparaître
        for (float t = 0; t < flashDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1, 0, t / flashDuration);
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        flashImage.color = new Color(1, 1, 1, 0); // Rend l'image complètement transparente
        isFlashing = false;
    }
}
