using System.Collections.Generic;
using UnityEngine;

/// <summary>Cấu hình độ khó: bảng đối tượng, nhịp sinh theo thời gian, vùng sinh và giới hạn số lượng.</summary>
[CreateAssetMenu(menuName = "SpaceShooter/Spawn Config")]
public class SpawnConfig : ScriptableObject
{
    [Tooltip("Danh sách đối tượng có thể sinh")]
    public List<SpawnEntry> entries = new List<SpawnEntry>();

    [Tooltip("Khoảng cách giữa 2 lần sinh (giây). Trục X = ElapsedTime / surviveDuration (0..1)")]
    public AnimationCurve spawnIntervalOverTime = CreateDefaultCurve();

    [Tooltip("Khoảng cách tối thiểu từ mép camera tới điểm sinh")]
    public float minSpawnMargin = 1.5f;
    [Tooltip("Khoảng cách tối đa từ mép camera tới điểm sinh")]
    public float maxSpawnMargin = 4f;
    [Tooltip("Số đối tượng tối đa cùng lúc")]
    public int maxAlive = 45;

    /// <summary>Đường cong mặc định (0,1.4) (0.5,0.8) (0.85,0.45) (1,0.35), nội suy tuyến tính giữa các mốc.</summary>
    private static AnimationCurve CreateDefaultCurve()
    {
        float[] times = { 0f, 0.5f, 0.85f, 1f };
        float[] values = { 1.4f, 0.8f, 0.45f, 0.35f };

        Keyframe[] keys = new Keyframe[times.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            // Độ dốc đoạn trước và đoạn sau mỗi mốc để đường cong thẳng giữa các điểm.
            float inSlope = i > 0
                ? (values[i] - values[i - 1]) / (times[i] - times[i - 1])
                : 0f;
            float outSlope = i < keys.Length - 1
                ? (values[i + 1] - values[i]) / (times[i + 1] - times[i])
                : 0f;
            keys[i] = new Keyframe(times[i], values[i], inSlope, outSlope);
        }
        return new AnimationCurve(keys);
    }

    private void Reset()
    {
        spawnIntervalOverTime = CreateDefaultCurve();
    }
}
