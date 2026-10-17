using UnityEngine;

/// <summary>Kẻ địch dùng chung một class, hành vi chọn theo EnemyData.pattern.</summary>
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData data;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explodeSfx;
    [SerializeField] private AudioClip shootSfx;

    private const float MuzzleOffset = 0.5f;    // đạn xuất hiện cách tâm địch một đoạn phía trước
    private const float OrbitArriveTolerance = 0.2f; // sai số khoảng cách coi là "đã tới nơi"

    private int health;
    private bool isDead;
    private HitFlash hitFlash;

    private Vector2 heading;      // hướng ban đầu (Straight)
    private float age;            // thời gian sống kể từ khi bật
    private float fireTimer;      // đếm ngược tới phát bắn kế tiếp (Shooter)
    private float orbitDirection; // +1 hoặc -1: chiều bay vòng (Shooter)
    private bool hasArrived;      // Shooter đã tới khoảng cách mong muốn

    private void Awake()
    {
        hitFlash = GetComponent<HitFlash>();
    }

    // Đối tượng pool: khởi tạo lại trạng thái mỗi lần được bật.
    private void OnEnable()
    {
        if (data == null)
        {
            Debug.LogWarning("Enemy: chưa gán EnemyData.", this);
            return;
        }

        health = data.maxHealth;
        isDead = false;
        age = 0f;
        hasArrived = false;
        fireTimer = data.fireInterval;
        orbitDirection = Random.value < 0.5f ? -1f : 1f;

        Vector2 toPlayer = GetPlayerPosition() - (Vector2)transform.position;
        if (toPlayer.sqrMagnitude < 0.0001f) toPlayer = Random.insideUnitCircle.normalized;
        heading = Direction2D.Rotate(toPlayer.normalized, Random.Range(-data.aimSpread, data.aimSpread));
        transform.rotation = Direction2D.ToRotation(heading);
    }

    private void Update()
    {
        if (data == null || isDead) return;

        age += Time.deltaTime;

        switch (data.pattern)
        {
            case MovementPattern.Straight: MoveStraight(); break;
            case MovementPattern.Shooter: MoveShooter(); break;
            case MovementPattern.Kamikaze: MoveKamikaze(); break;
        }

        if (Arena.Instance != null && Arena.Instance.ShouldDespawn(transform.position))
            Release();
    }

    /// <summary>
    /// Straight: bay thẳng theo hướng ban đầu.
    /// Công thức: position += heading * moveSpeed * dt (heading cố định từ OnEnable, không đổi hướng).
    /// </summary>
    private void MoveStraight()
    {
        transform.position += (Vector3)(heading * data.moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Shooter: tiến lại gần, bay vòng quanh người chơi, bắn, hết giờ thì bay đi.
    /// Công thức:
    ///  - offset = vị trí địch - vị trí người chơi; dist = |offset|.
    ///  - Chưa tới (dist > preferredDistance): tiến thẳng tới người chơi với moveSpeed.
    ///  - Đã tới và còn trong stayDuration: xoay offset một góc orbitDirection * orbitSpeed * dt
    ///    quanh người chơi (Direction2D.Rotate), đồng thời kéo bán kính về preferredDistance bằng MoveTowards.
    ///  - age > stayDuration: bay ra xa theo hướng offset với moveSpeed.
    ///  - Luôn xoay mặt về người chơi; chỉ bắn khi còn đang ở lại (mỗi fireInterval giây).
    /// </summary>
    private void MoveShooter()
    {
        Vector2 playerPos = GetPlayerPosition();
        Vector2 offset = (Vector2)transform.position - playerPos;
        float dist = offset.magnitude;
        if (dist < 0.0001f)
        {
            offset = Random.insideUnitCircle.normalized;
            dist = 0.0001f;
        }
        Vector2 outDir = offset / dist; // hướng từ người chơi ra địch

        bool leaving = age > data.stayDuration;
        Vector2 newPos;

        if (leaving)
        {
            newPos = (Vector2)transform.position + outDir * data.moveSpeed * Time.deltaTime;
        }
        else if (!hasArrived)
        {
            if (dist <= data.preferredDistance + OrbitArriveTolerance) hasArrived = true;
            newPos = (Vector2)transform.position - outDir * data.moveSpeed * Time.deltaTime;
        }
        else
        {
            float newDist = Mathf.MoveTowards(dist, data.preferredDistance, data.moveSpeed * Time.deltaTime);
            Vector2 newOffset = Direction2D.Rotate(outDir, orbitDirection * data.orbitSpeed * Time.deltaTime) * newDist;
            newPos = playerPos + newOffset;
        }

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        // Luôn quay mặt về người chơi.
        Vector2 toPlayer = playerPos - (Vector2)transform.position;
        if (toPlayer.sqrMagnitude > 0.0001f)
            transform.rotation = Direction2D.ToRotation(toPlayer);

        if (!leaving && hasArrived)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                fireTimer += data.fireInterval;
                Fire(toPlayer);
            }
        }
    }

    /// <summary>
    /// Kamikaze: trôi chậm rồi bám đuổi có giới hạn góc xoay.
    /// Công thức:
    ///  - age < dashDelay: đi về phía người chơi với moveSpeed và quay mặt theo hướng đó.
    ///  - sau đó: rotation = RotateTowards(rotation hiện tại, ToRotation(tới người chơi), turnSpeed * dt),
    ///    rồi position += transform.up * dashSpeed * dt. Vì góc xoay bị giới hạn nên lướt/vòng gấp sẽ né được.
    /// </summary>
    private void MoveKamikaze()
    {
        Vector2 toPlayer = GetPlayerPosition() - (Vector2)transform.position;
        Vector2 dirToPlayer = toPlayer.sqrMagnitude > 0.0001f ? toPlayer.normalized : (Vector2)transform.up;

        if (age < data.dashDelay)
        {
            transform.rotation = Direction2D.ToRotation(dirToPlayer);
            transform.position += (Vector3)(dirToPlayer * data.moveSpeed * Time.deltaTime);
            return;
        }

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Direction2D.ToRotation(dirToPlayer),
            data.turnSpeed * Time.deltaTime);
        transform.position += transform.up * (data.dashSpeed * Time.deltaTime);
    }

    // Bắn một viên về phía người chơi.
    private void Fire(Vector2 direction)
    {
        if (data.bulletPrefab == null || PoolManager.Instance == null) return;
        if (direction.sqrMagnitude < 0.0001f) return;

        Vector2 dir = direction.normalized;
        PoolManager.Instance.Spawn(
            data.bulletPrefab,
            transform.position + (Vector3)(dir * MuzzleOffset),
            Direction2D.ToRotation(dir));
        AudioManager.PlaySfx(shootSfx);
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

    /// <summary>Nổ. Nếu do người chơi bắn hạ: cộng điểm, đếm kill, rơi đồ.</summary>
    private void Die(bool killedByPlayer)
    {
        if (isDead) return;
        isDead = true;

        if (PoolManager.Instance != null && explosionPrefab != null)
            PoolManager.Instance.Spawn(explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.PlaySfx(explodeSfx);

        if (killedByPlayer && data != null)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(data.scoreValue);
                GameManager.Instance.AddKill();
            }
            if (LootManager.Instance != null) LootManager.Instance.TryDrop(transform.position, data.dropChance);
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
}
