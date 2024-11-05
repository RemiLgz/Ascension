using System.Collections.Generic;
using UnityEngine;

public class ImageLoop : MonoBehaviour
{
    public List<Transform> images; // Les trois images à faire boucler
    public float speed = 5f;       // Vitesse de déplacement des images
    public float resetPositionX = -10f;  // Position X où l'image revient derrière la dernière

    void Update()
    {
        // Parcourir toutes les images pour les déplacer et vérifier leur position
        foreach (Transform image in images)
        {
            // Déplacer l'image vers la gauche
            image.Translate(Vector3.left * speed * Time.deltaTime);

            // Si l'image atteint la position de réinitialisation, on la replace à droite derrière la dernière image
            if (image.position.x <= resetPositionX)
            {
                // Trouver la position X maximale parmi les images pour savoir où placer cette image
                float maxPosX = FindMaxXPosition();

                // Placer cette image à la suite de la dernière avec un espacement
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
