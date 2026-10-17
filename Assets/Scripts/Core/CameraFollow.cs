using UnityEngine;

/// <summary>Camera đi theo mục tiêu, làm mượt, nhìn trước, không đi quá viền bản đồ. Gắn trên CameraRig.</summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Tooltip("Tùy chọn: dùng hướng ngắm để nhìn trước. Để trống thì không nhìn trước")]
    [SerializeField] private PlayerAim playerAim;
    [Tooltip("Thời gian làm mượt (giây)")]
    [SerializeField] private float smoothTime = 0.15f;
    [Tooltip("Khoảng nhìn trước theo hướng ngắm")]
    [SerializeField] private float lookAhead = 1.5f;
    [Tooltip("Cho phép camera lố ra ngoài viền bản đồ bấy nhiêu đơn vị")]
    [SerializeField] private float edgeOverscroll = 1f;

    private Camera cam;
    private Vector3 velocity;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (cam == null) cam = Camera.main;
        if (cam == null) Debug.LogWarning("CameraFollow: không tìm thấy Camera.", this);

        // Đặt ngay vào vị trí mong muốn để không bị trượt từ gốc tọa độ.
        if (target != null && target.gameObject.activeInHierarchy)
            transform.position = ComputeDesiredPosition();
    }

    private void LateUpdate()
    {
        if (target == null || !target.gameObject.activeInHierarchy) return;

        Vector3 desired = ComputeDesiredPosition();
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    private Vector3 ComputeDesiredPosition()
    {
        Vector2 aimDirection = playerAim != null ? playerAim.AimDirection : Vector2.zero;
        Vector2 pos = (Vector2)target.position + aimDirection * lookAhead;
        pos = ClampToArena(pos);
        return new Vector3(pos.x, pos.y, transform.position.z);
    }

    /// <summary>Kẹp tâm camera sao cho vùng nhìn nằm trong Arena.Bounds mở rộng edgeOverscroll.</summary>
    private Vector2 ClampToArena(Vector2 pos)
    {
        Arena arena = Arena.Instance;
        if (arena == null || cam == null) return pos;

        Rect b = arena.Bounds;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = b.xMin - edgeOverscroll + halfW;
        float maxX = b.xMax + edgeOverscroll - halfW;
        float minY = b.yMin - edgeOverscroll + halfH;
        float maxY = b.yMax + edgeOverscroll - halfH;

        // Bản đồ nhỏ hơn camera theo trục nào thì căn giữa trục đó.
        pos.x = minX > maxX ? b.center.x : Mathf.Clamp(pos.x, minX, maxX);
        pos.y = minY > maxY ? b.center.y : Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}
