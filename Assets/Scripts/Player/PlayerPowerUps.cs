using UnityEngine;

/// <summary>Áp dụng hiệu ứng vật phẩm lên người chơi (Triple Shot có hẹn giờ, Shield, Extra Life).</summary>
public class PlayerPowerUps : MonoBehaviour
{
    [Tooltip("Thời gian hiệu lực của Triple Shot (giây)")]
    [SerializeField] private float effectDuration = 8f;

    private PlayerShooting shooting;
    private PlayerHealth health;
    private float tripleShotRemaining;

    public event System.Action<float> TripleShotStarted;
    public event System.Action TripleShotEnded;

    private void Awake()
    {
        shooting = GetComponent<PlayerShooting>();
        health = GetComponent<PlayerHealth>();
        if (shooting == null)
            Debug.LogWarning("PlayerPowerUps: thiếu PlayerShooting trên cùng object, Triple Shot sẽ không hoạt động.", this);
        if (health == null)
            Debug.LogWarning("PlayerPowerUps: thiếu PlayerHealth trên cùng object, Shield và Extra Life sẽ không hoạt động.", this);
    }

    private void OnEnable()
    {
        tripleShotRemaining = 0f;
        if (shooting != null) shooting.SetTripleShot(false);
    }

    private void Update()
    {
        if (tripleShotRemaining <= 0f) return;

        tripleShotRemaining -= Time.deltaTime;
        if (tripleShotRemaining <= 0f)
        {
            if (shooting != null) shooting.SetTripleShot(false);
            TripleShotEnded?.Invoke();
        }
    }

    /// <summary>Áp dụng vật phẩm. Nhặt lại Triple Shot khi đang có hiệu lực thì làm mới thời gian.</summary>
    public void Apply(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.TripleShot:
                tripleShotRemaining = effectDuration;
                if (shooting != null) shooting.SetTripleShot(true);
                TripleShotStarted?.Invoke(effectDuration);
                break;
            case PowerUpType.Shield:
                if (health != null) health.ActivateShield();
                break;
            case PowerUpType.ExtraLife:
                if (health != null) health.AddLife();
                break;
        }
    }
}
