using System.Collections.Generic;
using UnityEngine;

public class ImageLoop : MonoBehaviour
{
    public List<Transform> images; // Les trois images � faire boucler
    public float speed = 5f;       // Vitesse de d�placement des images
    public float resetPositionX = -10f;  // Position X o� l'image revient derri�re la derni�re

    void Update()
    {
        // Parcourir toutes les images pour les d�placer et v�rifier leur position
        foreach (Transform image in images)
        {
            // D�placer l'image vers la gauche
            image.Translate(Vector3.left * speed * Time.deltaTime);

            // Si l'image atteint la position de r�initialisation, on la replace � droite derri�re la derni�re image
            if (image.position.x <= resetPositionX)
            {
                // Trouver la position X maximale parmi les images pour savoir o� placer cette image
                float maxPosX = FindMaxXPosition();

                // Placer cette image � la suite de la derni�re avec un espacement
                image.position = new Vector3(maxPosX + image.GetComponent<Renderer>().bounds.size.x, image.position.y, image.position.z);
            }
        }
    }

    // Fonction pour trouver la position X maximale parmi toutes les images
    private float FindMaxXPosition()
    {
        float maxPosX = float.MinValue;
        foreach (Transform image in images)
        {
            if (image.position.x > maxPosX)
                maxPosX = image.position.x;
        }
        return maxPosX;
    }
}
