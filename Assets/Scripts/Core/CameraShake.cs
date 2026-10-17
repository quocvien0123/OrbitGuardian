using System.Collections;
using UnityEngine;

/// <summary>Rung camera bằng localPosition. Gắn trên Main Camera (con của CameraRig).</summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private Coroutine running;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OnDisable()
    {
        running = null;
        transform.localPosition = Vector3.zero;
    }

    /// <summary>Rung trong duration giây với biên độ magnitude, xong trả về (0,0,0).</summary>
    public void Shake(float duration = 0.2f, float magnitude = 0.15f)
    {
        if (!isActiveAndEnabled) return;
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector2 offset = Random.insideUnitCircle * magnitude;
            transform.localPosition = new Vector3(offset.x, offset.y, 0f);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        running = null;
    }
}
