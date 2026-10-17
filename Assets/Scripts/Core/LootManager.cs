using UnityEngine;

/// <summary>Rơi vật phẩm khi hạ địch / thiên thạch.</summary>
public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    [SerializeField] private GameObject[] powerUpPrefabs;

    private bool warnedEmpty;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    /// <summary>Với xác suất chance (0..1), sinh một vật phẩm ngẫu nhiên tại position.</summary>
    public void TryDrop(Vector3 position, float chance)
    {
        if (Random.value >= chance) return;

        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            if (!warnedEmpty)
            {
                Debug.LogWarning("LootManager: chưa gán Power Up Prefabs, không có vật phẩm nào rơi.", this);
                warnedEmpty = true;
            }
            return;
        }

        GameObject prefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        if (prefab == null || PoolManager.Instance == null) return;
        PoolManager.Instance.Spawn(prefab, position, Quaternion.identity);
    }
}
