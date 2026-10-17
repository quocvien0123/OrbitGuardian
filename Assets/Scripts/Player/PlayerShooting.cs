using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>Bắn đạn: giữ chuột trái / Space, hoặc tự bắn khi tự ngắm có mục tiêu. Hỗ trợ Triple Shot.</summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [Tooltip("Vị trí và hướng đạn xuất hiện (mũi tàu)")]
    [SerializeField] private Transform firePoint;
    [Tooltip("Giây giữa 2 phát bắn")]
    [SerializeField] private float fireRate = 0.15f;
    [Tooltip("Góc lệch của 2 viên phụ khi Triple Shot")]
    [SerializeField] private float tripleShotAngle = 15f;
    [Tooltip("Tự ngắm bắn chậm hơn: khoảng thời gian bắn chia cho hệ số này")]
    [SerializeField] private float autoAimFireRateMultiplier = 0.8f;
    [SerializeField] private AudioClip shootSfx;

    private PlayerAim playerAim;
    private bool tripleShot;
    private float nextFireTime;

    private void Awake()
    {
        playerAim = GetComponent<PlayerAim>();
        if (playerAim == null)
            Debug.LogWarning("PlayerShooting: thiếu PlayerAim trên cùng object, sẽ không tự bắn được.", this);
    }

    private void Start()
    {
        if (bulletPrefab == null)
            Debug.LogWarning("PlayerShooting: chưa gán Bullet Prefab.", this);
        if (firePoint == null)
            Debug.LogWarning("PlayerShooting: chưa gán Fire Point, dùng vị trí của tàu.", this);
    }

    private void OnEnable()
    {
        nextFireTime = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;
        if (Time.time < nextFireTime) return;
        if (!WantsToFire()) return;

        bool autoAim = playerAim != null && playerAim.IsAutoAim;
        float interval = autoAim && autoAimFireRateMultiplier > 0f
            ? fireRate / autoAimFireRateMultiplier
            : fireRate;
        nextFireTime = Time.time + interval;
        Fire();
    }

    private bool WantsToFire()
    {
        Mouse mouse = Mouse.current;
        Keyboard kb = Keyboard.current;
        if (mouse != null && mouse.leftButton.isPressed) return true;
        if (kb != null && kb.spaceKey.isPressed) return true;
        return playerAim != null && playerAim.IsAutoAim && playerAim.CurrentTarget != null;
    }

    private void Fire()
    {
        if (bulletPrefab == null || PoolManager.Instance == null) return;

        Transform origin = firePoint != null ? firePoint : transform;
        Vector3 pos = origin.position;
        Quaternion rot = origin.rotation;

        PoolManager.Instance.Spawn(bulletPrefab, pos, rot);
        if (tripleShot)
        {
            PoolManager.Instance.Spawn(bulletPrefab, pos, rot * Quaternion.Euler(0f, 0f, tripleShotAngle));
            PoolManager.Instance.Spawn(bulletPrefab, pos, rot * Quaternion.Euler(0f, 0f, -tripleShotAngle));
        }
        AudioManager.PlaySfx(shootSfx);
    }

    /// <summary>Bật/tắt Triple Shot (gọi từ PlayerPowerUps).</summary>
    public void SetTripleShot(bool active)
    {
        tripleShot = active;
    }
}
