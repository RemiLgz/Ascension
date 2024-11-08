using UnityEngine;
using System.Collections.Generic;

public class CubeEmitter : MonoBehaviour
{
    public List<GameObject> cubePrefabs; // Liste des diff�rents prefabs � �mettre
    public float spawnInterval = 2f; // Intervalle entre chaque �mission de prefab
    public float minSpeed = 0.2f; // Vitesse minimale du mouvement
    public float maxSpeed = 0.5f; // Vitesse maximale du mouvement
    public float prefabLifetime = 5f; // Dur�e de vie du prefab avant destruction

    private void Start()
    {
        InvokeRepeating("EmitPrefab", 0f, spawnInterval); // Appelle EmitPrefab toutes les "spawnInterval" secondes
    }

    private void EmitPrefab()
    {
        if (cubePrefabs.Count == 0) return; // S'assure qu'il y a des prefabs dans la liste

        // S�lectionne un prefab al�atoire dans la liste
        GameObject prefabToSpawn = cubePrefabs[Random.Range(0, cubePrefabs.Count)];

        // Instancie le prefab � la position de l'�metteur
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);

        // D�termine une direction et une vitesse al�atoires
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        // Ajoute une force au prefab pour le faire se d�placer
        Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = spawnedPrefab.AddComponent<Rigidbody>();
        }
        rb.useGravity = false; // Emp�che le prefab de tomber
        rb.velocity = randomDirection * randomSpeed;

        // D�truit le prefab apr�s sa dur�e de vie
        Destroy(spawnedPrefab, prefabLifetime);
    }
}
