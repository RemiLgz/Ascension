using UnityEngine;
using UnityEngine.UI;

public class SetPowerIncrease : MonoBehaviour
{
    public EnemyDestroyer enemyDestroyer;
    public Button setPowerIncreaseButton;

    void Start()
    {
        if (setPowerIncreaseButton != null && enemyDestroyer != null)
        {
            setPowerIncreaseButton.onClick.AddListener(SetPowerIncreaseTo100);
        }
    }

    void SetPowerIncreaseTo100()
    {
        if (enemyDestroyer != null)
        {
            enemyDestroyer.powerIncrease = (int)200f;
            Debug.Log("PowerIncrease has been set to 100");
        }
    }
}
