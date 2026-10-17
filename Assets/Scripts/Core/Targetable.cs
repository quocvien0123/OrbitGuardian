using System.Collections.Generic;
using UnityEngine;

/// <summary>Đánh dấu đối tượng có thể bị tự ngắm (Enemy, Asteroid, Mine). Tự đăng ký khi bật.</summary>
public class Targetable : MonoBehaviour
{
    private static readonly List<Targetable> all = new List<Targetable>();

    public static IReadOnlyList<Targetable> All => all;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        all.Clear();
    }

    private void OnEnable()
    {
        all.Add(this);
    }

    private void OnDisable()
    {
        all.Remove(this);
    }

    /// <summary>Mục tiêu gần nhất trong bán kính range, hoặc null.</summary>
    public static Targetable FindNearest(Vector3 from, float range)
    {
        Targetable best = null;
        float bestSqr = range * range;
        for (int i = 0; i < all.Count; i++)
        {
            float sqr = (all[i].transform.position - from).sqrMagnitude;
            if (sqr <= bestSqr)
            {
                bestSqr = sqr;
                best = all[i];
            }
        }
        return best;
    }
}
