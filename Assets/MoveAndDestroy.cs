using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    public float moveSpeed = 1f;  // Vitesse de d�placement sur l'axe -X
    public float destroyTime = 20f;  // Temps avant la destruction de l'objet

    void Start()
    {
        // Commencer le compte � rebours de destruction d�s le d�but
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        // D�place le GameObject et tous ses enfants vers la gauche (-x)
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // D�place �galement les enfants dans la m�me direction
        foreach (Transform child in transform)
        {
            child.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
    }
}
