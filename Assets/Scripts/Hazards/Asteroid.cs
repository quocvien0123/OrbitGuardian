using UnityEngine;

/// <summary>Thiên thạch trôi xuyên qua vùng quanh người chơi, tự xoay, vỡ thành mảnh nhỏ hơn khi bị bắn hạ.</summary>
public class Asteroid : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int scoreValue = 30;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 2.5f;
    [Tooltip("Bán kính lệch ngẫu nhiên quanh vị trí người chơi khi chọn hướng bay")]
    [SerializeField] private float aimOffsetRadius = 5f;
    [Tooltip("Tốc độ tự xoay tối đa (độ/giây), chọn ngẫu nhiên trong ±giá trị này")]
    [SerializeField] private float maxRotationSpeed = 90f;
    [Tooltip("Prefab mảnh vỡ (để trống nếu là thiên thạch nhỏ nhất)")]
    [SerializeField] private GameObject splitPrefab;
    [SerializeField] private int splitCount = 2;
    [Tooltip("Tốc độ của mảnh vỡ")]
    [SerializeField] private float splitSpeed = 2.5f;
    [SerializeField] private float dropChance = 0.05f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explodeSfx;

    private const float SplitSpreadAngle = 45f; // mảnh ngoài cùng lệch ±45° so với hướng bay

    private Vector2 velocity;
    private float spinSpeed;
    private int health;
    private bool isDead;
    private HitFlash hitFlash;

    private void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
    }

    // Đối tượng pool: khởi tạo lại trạng thái mỗi lần được bật.
    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;

        Vector2 target = GetPlayerPosition() + Random.insideUnitCircle * aimOffsetRadius;
        Vector2 dir = target - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;
        velocity = dir.normalized * Random.Range(minSpeed, maxSpeed);

        spinSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        if (Arena.Instance != null && Arena.Instance.ShouldDespawn(transform.position))
            Release();
    }

    /// <summary>Ghi đè vận tốc (dùng cho mảnh vỡ sau khi Spawn).</summary>
    public void Launch(Vector2 newVelocity)
    {
        velocity = newVelocity;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        if (hitFlash != null) hitFlash.Flash();
        health -= amount;
        if (health <= 0) Die(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null) return;

        player.TakeDamage(1);
        Die(false);
    }

    /// <summary>Nổ. Nếu do người chơi bắn hạ: cộng điểm, đếm kill, rơi đồ và sinh mảnh vỡ.</summary>
    private void Die(bool killedByPlayer)
    {
        if (isDead) return;
        isDead = true;

        if (PoolManager.Instance != null && explosionPrefab != null)
            PoolManager.Instance.Spawn(explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.PlaySfx(explodeSfx);

        if (killedByPlayer)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
                GameManager.Instance.AddKill();
            }
            if (LootManager.Instance != null) LootManager.Instance.TryDrop(transform.position, dropChance);
            SpawnFragments();
        }

        Release();
    }

    /// <summary>Sinh splitCount mảnh, tỏa đều theo góc quanh hướng bay hiện tại.</summary>
    private void SpawnFragments()
    {
        if (splitPrefab == null || splitCount <= 0 || PoolManager.Instance == null) return;

        Vector2 baseDir = velocity.sqrMagnitude > 0.0001f ? velocity.normalized : Random.insideUnitCircle.normalized;

        for (int i = 0; i < splitCount; i++)
        {
            // 1 mảnh: bay thẳng; nhiều mảnh: chia đều từ -45° đến +45°.
            float angle = splitCount == 1
                ? 0f
                : Mathf.Lerp(-SplitSpreadAngle, SplitSpreadAngle, i / (float)(splitCount - 1));
            Vector2 fragDir = Direction2D.Rotate(baseDir, angle);

            GameObject frag = PoolManager.Instance.Spawn(splitPrefab, transform.position, Quaternion.identity);
            if (frag == null) continue;

            Asteroid asteroid = frag.GetComponent<Asteroid>();
            if (asteroid != null) asteroid.Launch(fragDir * splitSpeed);
        }
    }

    private void Release()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Despawn(gameObject);
        else Destroy(gameObject);
    }

    // Vị trí người chơi; không có thì dùng tâm camera.
    private static Vector2 GetPlayerPosition()
    {
        PlayerHealth p = PlayerHealth.Instance;
        if (p != null && p.gameObject.activeInHierarchy) return p.transform.position;
        return Arena.GetCameraView().center;
    }
}
