using UnityEngine;
using System.Collections.Generic;

public class CubeEmitter : MonoBehaviour
{
    public List<GameObject> cubePrefabs; // Liste des différents prefabs à émettre
    public float spawnInterval = 2f; // Intervalle entre chaque émission de prefab
    public float minSpeed = 0.2f; // Vitesse minimale du mouvement
    public float maxSpeed = 0.5f; // Vitesse maximale du mouvement
    public float prefabLifetime = 5f; // Durée de vie du prefab avant destruction

    private void Start()
    {
        InvokeRepeating("EmitPrefab", 0f, spawnInterval); // Appelle EmitPrefab toutes les "spawnInterval" secondes
    }

    private void EmitPrefab()
    {
        if (cubePrefabs.Count == 0) return; // S'assure qu'il y a des prefabs dans la liste

        // Sélectionne un prefab aléatoire dans la liste
        GameObject prefabToSpawn = cubePrefabs[Random.Range(0, cubePrefabs.Count)];

        // Instancie le prefab à la position de l'émetteur
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        // Détermine une direction et une vitesse aléatoires
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        // Ajoute une force au prefab pour le faire se déplacer
        Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnedPrefab.AddComponent<Rigidbody>();
        }
        rb.useGravity = false; // Empêche le prefab de tomber
        rb.velocity = randomDirection * randomSpeed;

        // Détruit le prefab après sa durée de vie
        Destroy(spawnedPrefab, prefabLifetime);
    }
}
