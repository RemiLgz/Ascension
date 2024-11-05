using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Référence au prefab de l'ennemi
    public GameObject enemyPrefab;

    // Position où l'ennemi apparaîtra
    public Transform spawnPoint;

    // Intervalle de temps entre les spawns
    public float spawnInterval = 2.0f;

    // Booléen pour activer ou désactiver le spawn d'ennemis
    public bool SpawnEnnemiActive = false;

    private float nextSpawnTime = 0.0f;

    void Update()
    {
        // Vérifier si le spawn est actif et si le temps est écoulé pour un nouveau spawn
        if (SpawnEnnemiActive && Time.time >= nextSpawnTime)
        {
            // Mettre à jour le prochain moment de spawn
            nextSpawnTime = Time.time + spawnInterval;

            // Créer un nouvel ennemi à la position de spawn avec une rotation de 45° vers la gauche
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Calculer la rotation avec un décalage de 45 degrés vers la gauche
        Quaternion rotationWithOffset = spawnPoint.rotation * Quaternion.Euler(0, -90, 0);

        // Instancier l'ennemi avec la rotation ajustée
        Instantiate(enemyPrefab, spawnPoint.position, rotationWithOffset);
    }
}
