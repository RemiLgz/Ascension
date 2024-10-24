using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    public GameObject[] objectsToSpawn;
    public float minSpawnTime = 1.0f;
    public float maxSpawnTime = 3.0f;
    public float speed = 2.0f;
    public float spawnXPosition = 10.0f;
    public float spawnYPosition = 0.0f;
    public float spawnZPosition = 0.0f; // Variable pour ajuster la position de spawn en Z
    public float destroyOffset = 1.0f;
    public float destroyXPosition = -10.0f;

    private float nextSpawnTime;

    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            ScheduleNextSpawn();
        }
    }

    void SpawnObject()
    {
        int randomIndex = Random.Range(0, objectsToSpawn.Length);
        // Utilise la position Z spécifiée dans l'Inspector
        GameObject newObject = Instantiate(objectsToSpawn[randomIndex], new Vector3(spawnXPosition, spawnYPosition, spawnZPosition), Quaternion.identity);
        newObject.AddComponent<ScrollingObject>().Initialize(speed, destroyXPosition + destroyOffset);
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    class ScrollingObject : MonoBehaviour
    {
        private float speed;
        private float destroyXPosition;

        public void Initialize(float scrollSpeed, float destroyPosition)
        {
            speed = scrollSpeed;
            destroyXPosition = destroyPosition;
        }

        void Update()
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (transform.position.x <= destroyXPosition)
            {
                Destroy(gameObject);
            }
        }
    }
}
