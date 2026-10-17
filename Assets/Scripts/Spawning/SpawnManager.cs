using System.Collections;
using UnityEngine;

/// <summary>Sinh thiên thạch, mìn, kẻ địch ngay ngoài tầm nhìn camera theo SpawnConfig.</summary>
public class SpawnManager : MonoBehaviour
{
    // Khoảng chờ tối thiểu để tránh vòng lặp quá dày nếu curve cấu hình sai.
    private const float MinInterval = 0.05f;

    [Tooltip("Cấu hình sinh đối tượng (SpawnConfig)")]
    [SerializeField] private SpawnConfig config;
    [Tooltip("Số giây chờ trước khi sinh đối tượng đầu tiên")]
    [SerializeField] private float startDelay = 1.5f;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogWarning("SpawnManager: chưa gán SpawnConfig, sẽ không sinh đối tượng nào.", this);
            return;
        }
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            GameManager gm = GameManager.Instance;
            if (gm == null) yield break;
            if (gm.State == GameState.GameOver || gm.State == GameState.Victory) yield break;

            // Tỉ lệ thời gian 0..1 (tổng thời gian = đã trôi + còn lại).
            float total = gm.ElapsedTime + gm.RemainingTime;
            float t = total > 0f ? Mathf.Clamp01(gm.ElapsedTime / total) : 0f;
            float interval = Mathf.Max(MinInterval, config.spawnIntervalOverTime.Evaluate(t));
            yield return new WaitForSeconds(interval);

            // Trạng thái có thể đổi trong lúc chờ: kiểm tra lại trước khi sinh.
            if (gm.State != GameState.Playing) continue;
            if (Targetable.All.Count >= config.maxAlive) continue;

            SpawnEntry entry = PickWeightedEntry(gm.ElapsedTime);
            if (entry == null) continue;

            if (Arena.Instance == null || PoolManager.Instance == null)
            {
                Debug.LogWarning("SpawnManager: thiếu Arena hoặc PoolManager trong scene.", this);
                continue;
            }

            Vector3 pos = Arena.Instance.GetSpawnPointOutsideView(config.minSpawnMargin, config.maxSpawnMargin);
            PoolManager.Instance.Spawn(entry.prefab, pos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Chọn ngẫu nhiên có trọng số trong các entry đã mở khóa (unlockTime &lt;= elapsed).
    /// Cách làm: cộng tổng trọng số, bốc một số ngẫu nhiên trong [0, tổng), rồi duyệt lần lượt
    /// và trừ dần trọng số; entry nào làm số bốc về dưới 0 thì được chọn.
    /// Entry nào có trọng số lớn thì "chiếm" đoạn dài hơn nên dễ trúng hơn.
    /// Trả về null nếu không có entry hợp lệ (chưa mở khóa, thiếu prefab hoặc weight &lt;= 0).
    /// </summary>
    private SpawnEntry PickWeightedEntry(float elapsed)
    {
        float totalWeight = 0f;
        SpawnEntry lastValid = null;
        for (int i = 0; i < config.entries.Count; i++)
        {
            SpawnEntry e = config.entries[i];
            if (!IsAvailable(e, elapsed)) continue;
            totalWeight += e.weight;
            lastValid = e;
        }
        if (lastValid == null) return null;

        float roll = Random.Range(0f, totalWeight);
        for (int i = 0; i < config.entries.Count; i++)
        {
            SpawnEntry e = config.entries[i];
            if (!IsAvailable(e, elapsed)) continue;
            roll -= e.weight;
            if (roll < 0f) return e;
        }
        // Phòng sai số số thực: trả về entry hợp lệ cuối cùng.
        return lastValid;
    }

    private static bool IsAvailable(SpawnEntry e, float elapsed)
    {
        return e != null && e.prefab != null && e.weight > 0f && e.unlockTime <= elapsed;
    }
}
