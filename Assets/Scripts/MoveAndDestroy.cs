using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    public float moveSpeed = 1f;  // Vitesse de déplacement sur l'axe -X
    public float destroyTime = 20f;  // Temps avant la destruction de l'objet

    void Start()
    {
        // Commencer le compte à rebours de destruction dès le début
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // Déplace le GameObject et tous ses enfants vers la gauche (-x)
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // Déplace également les enfants dans la même direction
        foreach (Transform child in transform)
        {
            child.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }
}
