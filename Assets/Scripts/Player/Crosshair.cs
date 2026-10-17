using UnityEngine;

/// <summary>Tâm ngắm thay con trỏ chuột. Đỏ và dính vào mục tiêu khi tự ngắm đang khóa.</summary>
public class Crosshair : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Color manualColor = Color.white;
    [SerializeField] private Color lockedColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private bool subscribed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (playerAim == null)
            Debug.LogWarning("Crosshair: chưa gán Player Aim, tâm ngắm sẽ không di chuyển.", this);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StateChanged += OnStateChanged;
            subscribed = true;
            ApplyState(GameManager.Instance.State);
        }
        else
        {
            ApplyState(GameState.Playing);
        }
    }

    private void OnDestroy()
    {
        if (subscribed && GameManager.Instance != null)
            GameManager.Instance.StateChanged -= OnStateChanged;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (playerAim == null) return;

        // LateUpdate để chạy sau khi camera đã di chuyển, tránh tâm ngắm lệch một frame.
        Targetable target = playerAim.CurrentTarget;
        bool locked = playerAim.IsAutoAim && target != null && target.gameObject.activeInHierarchy;

        Vector3 pos = locked ? target.transform.position : playerAim.MouseWorldPosition;
        pos.z = 0f;
        transform.position = pos;

        if (spriteRenderer != null)
            spriteRenderer.color = locked ? lockedColor : manualColor;
    }

    private void OnStateChanged(GameState state)
    {
        ApplyState(state);
    }

    /// <summary>Đang chơi: ẩn con trỏ hệ thống, hiện tâm ngắm. Trạng thái khác: ngược lại.</summary>
    private void ApplyState(GameState state)
    {
        bool playing = state == GameState.Playing;
        Cursor.visible = !playing;
        if (spriteRenderer != null) spriteRenderer.enabled = playing;
    }
}
