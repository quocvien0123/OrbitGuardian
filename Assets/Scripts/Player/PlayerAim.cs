using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Xoay tàu theo chuột hoặc theo mục tiêu gần nhất (tự ngắm, bật/tắt bằng Q).</summary>
public class PlayerAim : MonoBehaviour
{
    [Tooltip("Tốc độ xoay tối đa (độ/giây)")]
    [SerializeField] private float rotationSpeed = 900f;
    [Tooltip("Bán kính tìm mục tiêu khi tự ngắm")]
    [SerializeField] private float autoAimRange = 10f;
    [Tooltip("Khoảng thời gian giữa 2 lần tìm lại mục tiêu (giây)")]
    [SerializeField] private float retargetInterval = 0.1f;
    [SerializeField] private bool startWithAutoAim = false;
    [SerializeField] private AudioClip toggleSfx;

    public bool IsAutoAim { get; private set; }
    public Targetable CurrentTarget { get; private set; }

    /// <summary>Hướng mũi tàu hiện tại (transform.up). Dùng hướng đã xoay mượt để camera không giật.</summary>
    public Vector2 AimDirection => transform.up;

    /// <summary>
    /// Vị trí chuột trong tọa độ thế giới. Chuột cho ra tọa độ MÀN HÌNH (pixel), còn tàu nằm trong
    /// THẾ GIỚI, nên phải đổi qua Camera.ScreenToWorldPoint. Camera orthographic nên x,y không phụ
    /// thuộc độ sâu; ta ép z = 0 để cùng mặt phẳng 2D với tàu. Tính tại thời điểm gọi (không cache)
    /// vì camera di chuyển trong LateUpdate, cache sẽ bị lệch một frame.
    /// </summary>
    public Vector3 MouseWorldPosition
    {
        get
        {
            Mouse mouse = Mouse.current;
            if (cam == null || mouse == null) return transform.position;
            Vector3 world = cam.ScreenToWorldPoint(mouse.position.ReadValue());
            world.z = 0f;
            return world;
        }
    }

    public event Action<bool> AimModeChanged;

    private Camera cam;
    private float retargetTimer;

    private void Awake()
    {
        IsAutoAim = startWithAutoAim;
    }

    private void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogWarning("PlayerAim: không tìm thấy Main Camera (tag MainCamera).", this);
        AimModeChanged?.Invoke(IsAutoAim);
    }

    private void OnEnable()
    {
        CurrentTarget = null;
        retargetTimer = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;

        Keyboard kb = Keyboard.current;
        if (kb != null && kb.qKey.wasPressedThisFrame)
            ToggleAutoAim();

        UpdateTarget();
        RotateTowardsAimPoint();
    }

    private void ToggleAutoAim()
    {
        IsAutoAim = !IsAutoAim;
        CurrentTarget = null;
        retargetTimer = 0f; // tìm mục tiêu ngay frame này
        AudioManager.PlaySfx(toggleSfx);
        AimModeChanged?.Invoke(IsAutoAim);
    }

    private void UpdateTarget()
    {
        if (!IsAutoAim)
        {
            CurrentTarget = null;
            return;
        }

        // Mục tiêu bị tắt (đã chết / trả về pool) thì bỏ ngay, không đợi lần tìm kế tiếp.
        if (CurrentTarget != null && !CurrentTarget.gameObject.activeInHierarchy)
            CurrentTarget = null;

        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f)
        {
            retargetTimer = retargetInterval;
            CurrentTarget = Targetable.FindNearest(transform.position, autoAimRange);
        }
    }

    private void RotateTowardsAimPoint()
    {
        Vector3 aimPoint = (IsAutoAim && CurrentTarget != null)
            ? CurrentTarget.transform.position
            : MouseWorldPosition;

        Vector2 toAim = aimPoint - transform.position;
        if (toAim.sqrMagnitude < 0.0001f) return; // con trỏ ngay trên tàu: giữ nguyên hướng

        // Dùng RotateTowards thay vì gán thẳng rotation: giới hạn số độ xoay mỗi frame
        // (rotationSpeed * dt) nên mũi tàu quay mượt, không "búng" tức thời khi chuột
        // vung nhanh hoặc đổi mục tiêu; đồng thời luôn đi theo đường ngắn nhất (không
        // bị lỗi vòng 359° -> 0° như khi nội suy thủ công bằng góc Euler).
        Quaternion targetRotation = Direction2D.ToRotation(toAim);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
