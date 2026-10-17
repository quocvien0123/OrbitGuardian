using UnityEngine;

/// <summary>Đạn bay thẳng theo transform.up, gây sát thương cho IDamageable đầu tiên chạm phải.</summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private int damage = 1;
    [Tooltip("Thời gian sống tối đa (giây)")]
    [SerializeField] private float lifetime = 1.5f;

    private float age;
    private bool hasHit;

    // Đối tượng pool: khởi tạo lại trạng thái mỗi lần được bật.
    private void OnEnable()
    {
        age = 0f;
        hasHit = false;
    }

    private void Update()
    {
        transform.position += transform.up * (speed * Time.deltaTime);

        age += Time.deltaTime;
        if (age >= lifetime || (Arena.Instance != null && Arena.Instance.ShouldDespawn(transform.position)))
            Release();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return; // chỉ trúng 1 lần dù chạm nhiều collider cùng frame

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target == null) return;

        hasHit = true;
        target.TakeDamage(damage);
        Release();
    }

    private void Release()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Despawn(gameObject);
        else Destroy(gameObject);
    }
}
