using UnityEngine;

public class InfiniteBridge : MonoBehaviour
{
    public GameObject[] bridges;
    public float speed = 2.0f;
    public float disappearOffset = 0.5f;
    public float bridgeSpacing = 0.0f;  // Variable pour gérer l'espacement

    private float spriteWidth;
    private Camera mainCamera;

    void Start()
    {
        spriteWidth = bridges[0].GetComponent<SpriteRenderer>().bounds.size.x;
        mainCamera = Camera.main;
        AlignBridges();
    }

    void Update()
    {
        for (int i = 0; i < bridges.Length; i++)
        {
            bridges[i].transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (IsOutOfScreen(bridges[i]))
            {
                RepositionBridge(i);
            }
        }
    }

    void RepositionBridge(int index)
    {
        int lastIndex = GetRightmostBridgeIndex();
        Vector3 newPosition = bridges[lastIndex].transform.position;
        newPosition.x += spriteWidth + bridgeSpacing;  // Utilisation de la variable d'espacement
        bridges[index].transform.position = newPosition;
    }

    int GetRightmostBridgeIndex()
    {
        int rightmostIndex = 0;
        for (int i = 1; i < bridges.Length; i++)
        {
            if (bridges[i].transform.position.x > bridges[rightmostIndex].transform.position.x)
            {
                rightmostIndex = i;
            }
        }
        return rightmostIndex;
    }

    bool IsOutOfScreen(GameObject bridge)
    {
        float leftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float bridgeRightEdge = bridge.transform.position.x + (spriteWidth / 2);
        return bridgeRightEdge < (leftEdge - disappearOffset);
    }

    void AlignBridges()
    {
        for (int i = 1; i < bridges.Length; i++)
        {
            Vector3 newPosition = bridges[i - 1].transform.position;
            newPosition.x += spriteWidth + bridgeSpacing;  // Utilisation de la variable d'espacement
            bridges[i].transform.position = newPosition;
        }
    }
}
