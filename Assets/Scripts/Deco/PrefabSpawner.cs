using UnityEngine;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    // Liste de préfabs à instancier
    public List<GameObject> prefabs;

    // Intervalle de temps entre chaque spawn en secondes
    public float spawnInterval = 2f;

    // Degré d'orientation (rotation) de l'objet en degrés
    public Vector3 spawnRotation = Vector3.zero;

    // Variable pour garder une trace du temps écoulé
    private float timeSinceLastSpawn;

    void Start()
    {
        // Initialiser le compteur de temps
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        // Incrémenter le temps écoulé
        timeSinceLastSpawn += Time.deltaTime;

        // Vérifier si le temps écoulé est supérieur ou égal à l'intervalle de spawn
        if (timeSinceLastSpawn >= spawnInterval)
        {
            // Vérifier si la liste de préfabs n'est pas vide
            if (prefabs.Count > 0)
            {
                // Sélectionner un préfab aléatoirement dans la liste
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Count)];

                // Calculer la rotation spécifiée en Quaternion
                Quaternion rotation = Quaternion.Euler(spawnRotation);

                // Instancier le préfab choisi à la position du GameObject actuel avec la rotation définie
                Instantiate(prefabToSpawn, transform.position, rotation);
            }

            // Réinitialiser le compteur de temps
            timeSinceLastSpawn = 0f;
        }
    }
}
