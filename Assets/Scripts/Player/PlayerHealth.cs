using System;
using System.Collections;
using UnityEngine;

/// <summary>Mạng, khiên, bất tử tạm thời và cái chết của người chơi. Không tự xử lý va chạm.</summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    private const int ExtraLifeBonusScore = 200;
    private const float ShieldBreakInvincibility = 0.5f;

    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private int maxLives = 3;
    [Tooltip("Bất tử sau khi mất mạng (giây)")]
    [SerializeField] private float invincibleDuration = 2f;
    [Tooltip("Chu kỳ nhấp nháy (giây)")]
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private GameObject explosionPrefab;
    [Tooltip("Hình khiên bao quanh tàu (bật khi có khiên)")]
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    public int CurrentLives { get; private set; }
    public int MaxLives => maxLives;
    public bool HasShield { get; private set; }

    public event Action<int> LivesChanged;

    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private float invincibleUntil;
    private bool isDead;
    private Coroutine blinkRoutine;

    private void Awake()
    {
        Instance = this;
        CurrentLives = maxLives;
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void OnEnable()
    {
        blinkRoutine = null;
        if (spriteRenderer != null) spriteRenderer.enabled = true;
    }

    private void Start()
    {
        UpdateShieldVisual();
        LivesChanged?.Invoke(CurrentLives);
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.Instance == null || GameManager.Instance.State != GameState.Playing) return;
        if (isDead || Time.time < invincibleUntil) return;
        if (movement != null && movement.IsDashing) return;

        if (HasShield)
        {
            HasShield = false;
            UpdateShieldVisual();
            AudioManager.PlaySfx(hitSfx);
            BecomeInvincible(ShieldBreakInvincibility);
            return;
        }

        CurrentLives = Mathf.Max(0, CurrentLives - Mathf.Max(1, amount));
        LivesChanged?.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            Die();
            return;
        }

        AudioManager.PlaySfx(hitSfx);
        if (CameraShake.Instance != null) CameraShake.Instance.Shake();
        BecomeInvincible(invincibleDuration);
    }

    /// <summary>+1 mạng. Đã đủ mạng thì cộng 200 điểm.</summary>
    public void AddLife()
    {
        if (isDead) return;
        if (CurrentLives >= maxLives)
        {
            if (GameManager.Instance != null) GameManager.Instance.AddScore(ExtraLifeBonusScore);
            return;
        }
        CurrentLives++;
        LivesChanged?.Invoke(CurrentLives);
    }

    /// <summary>Bật khiên chặn 1 đòn.</summary>
    public void ActivateShield()
    {
        if (isDead) return;
        HasShield = true;
        UpdateShieldVisual();
    }

    private void Die()
    {
        isDead = true;
        HasShield = false;
        UpdateShieldVisual();

        if (explosionPrefab != null && PoolManager.Instance != null)
            PoolManager.Instance.Spawn(explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.PlaySfx(deathSfx);
        if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.4f, 0.3f);

        if (GameManager.Instance != null) GameManager.Instance.OnPlayerDied();
        gameObject.SetActive(false);
    }

    private void UpdateShieldVisual()
    {
        if (shieldVisual != null) shieldVisual.SetActive(HasShield);
    }

    /// <summary>Bất tử thêm duration giây (không rút ngắn thời gian bất tử đang có) và nhấp nháy.</summary>
    private void BecomeInvincible(float duration)
    {
        invincibleUntil = Mathf.Max(invincibleUntil, Time.time + duration);
        if (blinkRoutine == null && spriteRenderer != null)
            blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        var wait = new WaitForSeconds(blinkInterval);
        while (Time.time < invincibleUntil)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return wait;
        }
        spriteRenderer.enabled = true;
        blinkRoutine = null;
    }
}
