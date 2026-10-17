using UnityEngine;

/// <summary>Lớp nền lặp (tiled) có hiệu ứng parallax theo camera.</summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = gắn chặt với bản đồ, gần 1 = gần như đứng yên theo camera (rất xa)")]
    [SerializeField] private float parallaxFactor = 0.5f;
    [Tooltip("Tốc độ trôi của lớp nền (đơn vị/giây)")]
    [SerializeField] private Vector2 driftSpeed = Vector2.zero;

    private SpriteRenderer sr;
    private Camera cam;
    private Vector3 startPosition;
    private Vector2 drift;
    private Vector2 tileWorldSize;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogWarning("ParallaxLayer: không tìm thấy Main Camera.", this);
        if (sr.sprite == null)
        {
            Debug.LogWarning("ParallaxLayer: SpriteRenderer chưa có sprite.", this);
            enabled = false;
            return;
        }

        Vector2 localTile = sr.sprite.bounds.size;
        Vector3 scale = transform.lossyScale;
        tileWorldSize = new Vector2(localTile.x * Mathf.Abs(scale.x), localTile.y * Mathf.Abs(scale.y));

        // Kích thước phủ: Arena + 2 x vùng camera (không có Arena: 3 x vùng camera), cộng thêm 1 ô mỗi phía cho độ trôi.
        Vector2 view = Arena.GetCameraView().size;
        Vector2 worldSize = Arena.Instance != null
            ? Arena.Instance.Bounds.size + view * 2f
            : view * 3f;
        worldSize += tileWorldSize * 2f;

        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(
            worldSize.x / Mathf.Max(0.0001f, Mathf.Abs(scale.x)),
            worldSize.y / Mathf.Max(0.0001f, Mathf.Abs(scale.y)));

        startPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        drift += driftSpeed * Time.deltaTime;
        // Lấy phần dư theo kích thước một ô để không bao giờ lộ mép.
        if (tileWorldSize.x > 0f) drift.x %= tileWorldSize.x;
        if (tileWorldSize.y > 0f) drift.y %= tileWorldSize.y;

        Vector3 camPos = cam.transform.position;
        transform.position = new Vector3(
            startPosition.x + camPos.x * parallaxFactor + drift.x,
            startPosition.y + camPos.y * parallaxFactor + drift.y,
            startPosition.z);
    }
}
