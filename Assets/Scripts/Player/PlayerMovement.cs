using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Di chuyển WASD có quán tính và lướt né (Shift). Không xoay tàu (việc của PlayerAim).</summary>
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Tốc độ tối đa (đơn vị/giây)")]
    [SerializeField] private float maxSpeed = 7f;
    [Tooltip("Gia tốc khi có input")]
    [SerializeField] private float acceleration = 45f;
    [Tooltip("Giảm tốc khi thả phím")]
    [SerializeField] private float deceleration = 30f;
    [Tooltip("Khoảng cách giữ với viền bản đồ")]
    [SerializeField] private float arenaPadding = 0.6f;
    [Tooltip("Tốc độ lướt")]
    [SerializeField] private float dashSpeed = 20f;
    [Tooltip("Thời gian lướt (giây). 20 x 0.15 = 3 đơn vị")]
    [SerializeField] private float dashDuration = 0.15f;
    [Tooltip("Hồi chiêu lướt (giây), tính từ lúc bắt đầu lướt")]
    [SerializeField] private float dashCooldown = 1.2f;
    [Tooltip("Hiệu ứng khi lướt (tùy chọn)")]
    [SerializeField] private GameObject dashEffectPrefab;
    [SerializeField] private AudioClip dashSfx;

    public Vector2 Velocity { get; private set; }
    public bool IsDashing { get; private set; }
    /// <summary>1 = vừa dùng lướt, 0 = sẵn sàng.</summary>
    public float DashCooldownNormalized =>
        dashCooldown > 0f ? Mathf.Clamp01(cooldownTimer / dashCooldown) : 0f;

    private float cooldownTimer;
    private float dashTimer;
    private Vector2 dashDirection;

    private void OnEnable()
    {
        Velocity = Vector2.zero;
        IsDashing = false;
        cooldownTimer = 0f;
        dashTimer = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;

        float dt = Time.deltaTime;
        if (cooldownTimer > 0f) cooldownTimer -= dt;

        Vector2 input = ReadInput();

        if (!IsDashing && cooldownTimer <= 0f && DashPressed())
            StartDash(input);

        if (IsDashing)
        {
            Velocity = dashDirection * dashSpeed;
            dashTimer -= dt;
            if (dashTimer <= 0f)
            {
                IsDashing = false;
                // Kết thúc lướt: hạ về tốc độ thường để không "phóng" quá nhanh.
                Velocity = Vector2.ClampMagnitude(Velocity, maxSpeed);
            }
        }
        else if (input.sqrMagnitude > 0.0001f)
        {
            Velocity = Vector2.MoveTowards(Velocity, input * maxSpeed, acceleration * dt);
        }
        else
        {
            Velocity = Vector2.MoveTowards(Velocity, Vector2.zero, deceleration * dt);
        }

        ApplyMovement(dt);
    }

    /// <summary>Đọc WASD và chuẩn hóa để đi chéo không nhanh hơn đi thẳng.</summary>
    private static Vector2 ReadInput()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return Vector2.zero;

        Vector2 input = Vector2.zero;
        if (kb.wKey.isPressed) input.y += 1f;
        if (kb.sKey.isPressed) input.y -= 1f;
        if (kb.dKey.isPressed) input.x += 1f;
        if (kb.aKey.isPressed) input.x -= 1f;
        return input.normalized;
    }

    private static bool DashPressed()
    {
        Keyboard kb = Keyboard.current;
        return kb != null && (kb.leftShiftKey.wasPressedThisFrame || kb.rightShiftKey.wasPressedThisFrame);
    }

    private void StartDash(Vector2 input)
    {
        // Không có input thì lướt theo hướng mũi tàu.
        dashDirection = input.sqrMagnitude > 0.0001f ? input : (Vector2)transform.up;
        IsDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = dashCooldown;

        if (dashEffectPrefab != null && PoolManager.Instance != null)
            PoolManager.Instance.Spawn(dashEffectPrefab, transform.position, transform.rotation);
        AudioManager.PlaySfx(dashSfx);
    }

    private void ApplyMovement(float dt)
    {
        Vector3 newPos = transform.position + (Vector3)(Velocity * dt);

        if (Arena.Instance != null)
        {
            Vector3 clamped = Arena.Instance.ClampInside(newPos, arenaPadding);
            // Chạm viền theo trục nào thì triệt tiêu vận tốc trục đó.
            Vector2 v = Velocity;
            if (!Mathf.Approximately(clamped.x, newPos.x)) v.x = 0f;
            if (!Mathf.Approximately(clamped.y, newPos.y)) v.y = 0f;
            Velocity = v;
            newPos = clamped;
        }

        transform.position = newPos;
    }
}
