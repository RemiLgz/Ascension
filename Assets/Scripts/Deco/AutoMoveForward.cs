using UnityEngine;

public class AutoMoveForward : MonoBehaviour
{
    // Vitesse de déplacement (modifiable dans l'inspecteur)
    public float speed = 5.0f;

    void Update()
    {
        // Faire avancer le prefab en permanence dans la direction de son axe Z local
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
