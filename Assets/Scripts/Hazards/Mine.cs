using UnityEngine;

/// <summary>Mìn trôi rất chậm về phía người chơi, nổ gây sát thương trong bán kính.</summary>
public class Mine : MonoBehaviour, IDamageable
{
    private const float DirectionJitter = 30f; // lệch ngẫu nhiên ±30° so với hướng tới người chơi

    [SerializeField] private int maxHealth = 2;
    [SerializeField] private int scoreValue = 25;
    [SerializeField] private float speed = 0.6f;
    [Tooltip("Bán kính sát thương khi nổ")]
    [SerializeField] private float explosionRadius = 1.5f;
    [Tooltip("Layer của người chơi (Player)")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float dropChance = 0.05f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explodeSfx;

    private Vector2 velocity;
    private int health;
    private bool isDead;
    private HitFlash hitFlash;

    private void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
        if (playerLayer.value == 0)
            Debug.LogWarning("Mine: chưa chọn Player Layer, mìn nổ sẽ không gây sát thương lan.", this);
    }

    // Đối tượng pool: khởi tạo lại trạng thái mỗi lần được bật.
    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;

        Vector2 dir = GetPlayerPosition() - (Vector2)transform.position;
        if (dir.sqrMagnitude < 0.0001f) dir = Random.insideUnitCircle.normalized;
        velocity = Direction2D.Rotate(dir.normalized, Random.Range(-DirectionJitter, DirectionJitter)) * speed;
    }

    private void Update()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (Arena.Instance != null && Arena.Instance.ShouldDespawn(transform.position))
            Release();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        if (hitFlash != null) hitFlash.Flash();
        health -= amount;
        if (health <= 0) Explode(true, null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null) return;

        player.TakeDamage(1);
        Explode(false, player);
    }

    /// <summary>Nổ lan trong explosionRadius. alreadyHit: người chơi đã nhận sát thương do va chạm (tránh trừ 2 lần).</summary>
    private void Explode(bool killedByPlayer, PlayerHealth alreadyHit)
    {
        if (isDead) return;
        isDead = true;

        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if (hit != null)
        {
            PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
            if (player != null && player != alreadyHit) player.TakeDamage(1);
        }

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
        }

        Release();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
