using UnityEngine;

/// <summary>Vật phẩm lơ lửng nhấp nhô, nhấp nháy ở cuối đời, chạm người chơi thì nhặt.</summary>
public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType type;
    [Tooltip("Thời gian tồn tại (giây)")]
    [SerializeField] private float lifetime = 10f;
    [Tooltip("Số giây cuối nhấp nháy")]
    [SerializeField] private float blinkTime = 3f;
    [SerializeField] private float bobAmplitude = 0.15f;
    [SerializeField] private float bobFrequency = 3f;
    [SerializeField] private AudioClip pickupSfx;

    private const float BlinkInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Vector3 origin;
    private float age;
    private bool collected;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Đối tượng pool: khởi tạo lại trạng thái mỗi lần được bật (vị trí đã được đặt trước khi active).
    private void OnEnable()
    {
        origin = transform.position;
        age = 0f;
        collected = false;
        if (spriteRenderer != null) spriteRenderer.enabled = true;
    }

    private void Update()
    {
        age += Time.deltaTime;

        transform.position = origin + Vector3.up * (Mathf.Sin(age * bobFrequency) * bobAmplitude);

        float remaining = lifetime - age;
        if (remaining <= 0f)
        {
            Release();
            return;
        }

        if (spriteRenderer != null)
        {
            bool blinking = remaining <= blinkTime;
            spriteRenderer.enabled = !blinking || Mathf.FloorToInt(age / BlinkInterval) % 2 == 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        PlayerPowerUps player = other.GetComponentInParent<PlayerPowerUps>();
        if (player == null) return;

        collected = true;
        player.Apply(type);
        AudioManager.PlaySfx(pickupSfx);
        Release();
    }

    private void Release()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Despawn(gameObject);
        else Destroy(gameObject);
    }
}
