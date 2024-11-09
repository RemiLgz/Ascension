using UnityEngine;

public class CameraBreathEffect : MonoBehaviour
{
    public float breathIntensity = 0.1f;  // Intensit� du mouvement de la cam�ra
    public float breathSpeed = 0.5f;      // Vitesse de respiration
    public float zoomIntensity = 0.05f;   // Intensit� du zoom pendant la respiration
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialFieldOfView;

    void Start()
    {
        // Enregistrer la position, la rotation et le FOV initiaux
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialFieldOfView = Camera.main.fieldOfView;
    }

    void Update()
    {
        // Calculer le d�calage de respiration bas� sur le temps
        float breathOffset = Mathf.Sin(Time.time * breathSpeed);

        // Appliquer le mouvement de respiration � la position (avant-arri�re)
        transform.localPosition = initialPosition + new Vector3(0, 0, breathOffset * breathIntensity);

        // Ajouter une l�g�re rotation pour un effet de basculement
        transform.localRotation = initialRotation * Quaternion.Euler(breathOffset * breathIntensity * 10f, 0, 0);

        // Appliquer un effet de zoom de respiration
        Camera.main.fieldOfView = initialFieldOfView + (breathOffset * zoomIntensity);
    }
}
