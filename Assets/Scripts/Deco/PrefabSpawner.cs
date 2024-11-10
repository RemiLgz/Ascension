using UnityEngine;
using System.Collections.Generic;

public class PrefabSpawner : MonoBehaviour
{
    // Liste de pr�fabs � instancier
    public List<GameObject> prefabs;

    // Intervalle de temps entre chaque spawn en secondes
    public float spawnInterval = 2f;

    // Degr� d'orientation (rotation) de l'objet en degr�s
    public Vector3 spawnRotation = Vector3.zero;

    // Variable pour garder une trace du temps �coul�
    private float timeSinceLastSpawn;

    void Start()
    {
        // Initialiser le compteur de temps
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        // Incr�menter le temps �coul�
        timeSinceLastSpawn += Time.deltaTime;

        // V�rifier si le temps �coul� est sup�rieur ou �gal � l'intervalle de spawn
        if (timeSinceLastSpawn >= spawnInterval)
        {
            // V�rifier si la liste de pr�fabs n'est pas vide
            if (prefabs.Count > 0)
            {
                // S�lectionner un pr�fab al�atoirement dans la liste
                GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Count)];

                // Calculer la rotation sp�cifi�e en Quaternion
                Quaternion rotation = Quaternion.Euler(spawnRotation);

                // Instancier le pr�fab choisi � la position du GameObject actuel avec la rotation d�finie
                Instantiate(prefabToSpawn, transform.position, rotation);
            }

            // R�initialiser le compteur de temps
            timeSinceLastSpawn = 0f;
        }
    }
}
