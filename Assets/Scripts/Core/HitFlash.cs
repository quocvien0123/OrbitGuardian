using System.Collections;
using UnityEngine;

/// <summary>Nháy màu ngắn khi bị trúng đòn.</summary>
public class HitFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(1f, 0.4f, 0.4f);
    [SerializeField] private float duration = 0.08f;

    private SpriteRenderer sr;
    private Color originalColor = Color.white;
    private Coroutine running;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    private void OnEnable()
    {
        running = null;
        if (sr != null) sr.color = originalColor;
    }

    /// <summary>Đổi sang flashColor trong duration giây rồi trả màu gốc.</summary>
    public void Flash()
    {
        if (sr == null || !isActiveAndEnabled) return;
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
        running = null;
    }
}
