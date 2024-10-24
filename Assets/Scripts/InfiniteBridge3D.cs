using UnityEngine;

public class InfiniteBridge3D : MonoBehaviour
{
    public GameObject[] bridges; // Array of 3D bridge prefabs
    public float speed = 2.0f;
    public float disappearOffset = 0.5f;
    public float bridgeSpacing = 0.0f;  // Variable to manage spacing between bridges

    private float objectWidth; // Width of the 3D object
    private Camera mainCamera;

    void Start()
    {
        // Get the width of the first bridge's mesh
        objectWidth = bridges[0].GetComponent<MeshRenderer>().bounds.size.x;
        mainCamera = Camera.main;
        AlignBridges(); // Align the initial bridges in the scene
    }

    void Update()
    {
        // Move each bridge to the left
        for (int i = 0; i < bridges.Length; i++)
        {
            bridges[i].transform.Translate(Vector3.left * speed * Time.deltaTime);

            // If a bridge is out of the screen, reposition it
            if (IsOutOfScreen(bridges[i]))
            {
                RepositionBridge(i);
            }
        }
    }

    // Reposition the bridge to the rightmost position once it's offscreen
    void RepositionBridge(int index)
    {
        int lastIndex = GetRightmostBridgeIndex(); // Get the rightmost bridge
        Vector3 newPosition = bridges[lastIndex].transform.position;
        newPosition.x += objectWidth + bridgeSpacing; // Adjust position with spacing
        bridges[index].transform.position = newPosition;
    }

    // Find which bridge is farthest to the right
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

    // Check if a bridge is offscreen on the left side
    bool IsOutOfScreen(GameObject bridge)
    {
        // Get the left edge of the screen
        float leftEdge = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        // Calculate the right edge of the bridge
        float bridgeRightEdge = bridge.transform.position.x + (objectWidth / 2);
        return bridgeRightEdge < (leftEdge - disappearOffset);
    }

    // Align the bridges initially in the scene
    void AlignBridges()
    {
        for (int i = 1; i < bridges.Length; i++)
        {
            Vector3 newPosition = bridges[i - 1].transform.position;
            newPosition.x += objectWidth + bridgeSpacing; // Apply spacing
            bridges[i].transform.position = newPosition;
        }
    }
}
