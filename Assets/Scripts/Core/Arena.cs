using UnityEngine;

/// <summary>Bản đồ hình chữ nhật: biên, kẹp vị trí, điều kiện xóa đối tượng, điểm sinh ngoài tầm nhìn.</summary>
public class Arena : MonoBehaviour
{
    public static Arena Instance { get; private set; }

    [Tooltip("Kích thước bản đồ (đơn vị thế giới)")]
    [SerializeField] private Vector2 size = new Vector2(64f, 36f);
    [Tooltip("Đối tượng ra ngoài biên quá khoảng này thì bị xóa")]
    [SerializeField] private float despawnMargin = 3f;
    [Tooltip("Đối tượng cách tâm camera quá khoảng này thì bị xóa")]
    [SerializeField] private float despawnDistance = 30f;
    [Tooltip("Đường viền hiển thị (tùy chọn)")]
    [SerializeField] private LineRenderer border;

    /// <summary>Hình chữ nhật bản đồ, tâm tại vị trí của object.</summary>
    public Rect Bounds
    {
        get
        {
            Vector2 center = transform.position;
            return new Rect(center - size * 0.5f, size);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        if (border == null) return;
        Rect b = Bounds;
        float z = transform.position.z;
        border.positionCount = 4;
        border.SetPosition(0, new Vector3(b.xMin, b.yMin, z));
        border.SetPosition(1, new Vector3(b.xMin, b.yMax, z));
        border.SetPosition(2, new Vector3(b.xMax, b.yMax, z));
        border.SetPosition(3, new Vector3(b.xMax, b.yMin, z));
        border.loop = true;
        border.useWorldSpace = true;
    }

    /// <summary>Kẹp vị trí vào trong bản đồ, cách biên một khoảng padding. Giữ nguyên z.</summary>
    public Vector3 ClampInside(Vector3 pos, float padding)
    {
        Rect b = Bounds;
        pos.x = Mathf.Clamp(pos.x, b.xMin + padding, b.xMax - padding);
        pos.y = Mathf.Clamp(pos.y, b.yMin + padding, b.yMax - padding);
        return pos;
    }

    /// <summary>True nếu vị trí ra ngoài bản đồ (mở rộng despawnMargin) hoặc quá xa tâm camera.</summary>
    public bool ShouldDespawn(Vector3 pos)
    {
        Rect b = Bounds;
        b.xMin -= despawnMargin;
        b.yMin -= despawnMargin;
        b.xMax += despawnMargin;
        b.yMax += despawnMargin;
        if (!b.Contains(pos)) return true;

        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector2 offset = (Vector2)pos - (Vector2)cam.transform.position;
            if (offset.sqrMagnitude > despawnDistance * despawnDistance) return true;
        }
        return false;
    }

    /// <summary>Vùng camera chính đang nhìn thấy (theo thế giới), mở rộng margin.</summary>
    public static Rect GetCameraView(float margin = 0f)
    {
        Camera cam = Camera.main;
        if (cam == null) return new Rect(Vector2.zero, Vector2.zero);

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;
        Vector2 center = cam.transform.position;
        return new Rect(
            center.x - width * 0.5f - margin,
            center.y - height * 0.5f - margin,
            width + margin * 2f,
            height + margin * 2f);
    }

    /// <summary>Điểm ngẫu nhiên ngay ngoài tầm nhìn camera nhưng vẫn trong bản đồ.</summary>
    public Vector3 GetSpawnPointOutsideView(float minMargin, float maxMargin)
    {
        Rect inner = GetCameraView(minMargin);
        Rect outer = GetCameraView(maxMargin);
        Rect bounds = Bounds;

        for (int i = 0; i < 15; i++)
        {
            Vector2 p = new Vector2(
                Random.Range(outer.xMin, outer.xMax),
                Random.Range(outer.yMin, outer.yMax));
            if (!inner.Contains(p) && bounds.Contains(p))
                return new Vector3(p.x, p.y, 0f);
        }

        // Thất bại: lấy điểm trong bản đồ cách tâm camera ít nhất 12 đơn vị.
        Camera cam = Camera.main;
        Vector2 camPos = cam != null ? (Vector2)cam.transform.position : (Vector2)transform.position;
        Vector2 fallback = bounds.center;
        for (int i = 0; i < 30; i++)
        {
            fallback = new Vector2(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax));
            if ((fallback - camPos).sqrMagnitude >= 12f * 12f) break;
        }
        return new Vector3(fallback.x, fallback.y, 0f);
    }

    private void OnDrawGizmos()
    {
        Rect b = Bounds;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(b.center, b.size);
    }
}
