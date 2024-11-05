using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // R�f�rence au prefab de l'ennemi
    public GameObject enemyPrefab;

    // Position o� l'ennemi appara�tra
    public Transform spawnPoint;

    // Intervalle de temps entre les spawns
    public float spawnInterval = 2.0f;

    // Bool�en pour activer ou d�sactiver le spawn d'ennemis
    public bool SpawnEnnemiActive = false;

    private float nextSpawnTime = 0.0f;

    void Update()
    {
        // V�rifier si le spawn est actif et si le temps est �coul� pour un nouveau spawn
        if (SpawnEnnemiActive && Time.time >= nextSpawnTime)
        {
            // Mettre � jour le prochain moment de spawn
            nextSpawnTime = Time.time + spawnInterval;

            // Cr�er un nouvel ennemi � la position de spawn avec une rotation de 45� vers la gauche
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Calculer la rotation avec un d�calage de 45 degr�s vers la gauche
        Quaternion rotationWithOffset = spawnPoint.rotation * Quaternion.Euler(0, -90, 0);

        // Instancier l'ennemi avec la rotation ajust�e
        Instantiate(enemyPrefab, spawnPoint.position, rotationWithOffset);
    }
}
