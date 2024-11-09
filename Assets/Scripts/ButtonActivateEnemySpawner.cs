using UnityEngine;
using UnityEngine.UI;

public class ButtonActivateEnemySpawner : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Button button;

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(ActivateEnemySpawner);
        }
    }

    public void ActivateEnemySpawner()
    {
        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnnemiActive = true;
        }
    }
}
