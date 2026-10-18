using UnityEngine;
using UnityEngine.UI;

/// <summary>Hiển thị các hiệu ứng vật phẩm đang kích hoạt (Triple Shot có thanh thời lượng, Khiên bật/tắt).</summary>
public class ActiveEffectsHUD : MonoBehaviour
{
    [Tooltip("Tham chiếu tới PlayerPowerUps để nhận event thời lượng")]
    [SerializeField] private PlayerPowerUps playerPowerUps;

    [Tooltip("Tham chiếu tới PlayerHealth để nhận event trạng thái khiên")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Khung/ô chứa hiển thị vật phẩm Triple Shot")]
    [SerializeField] private GameObject tripleShotSlot;

    [Tooltip("Thanh Image fill hiển thị thời lượng còn lại của Triple Shot")]
    [SerializeField] private Image tripleShotFill;

    [Tooltip("Khung/ô chứa hiển thị vật phẩm Khiên (Shield)")]
    [SerializeField] private GameObject shieldSlot;

    private float tripleShotDuration;
    private float tripleShotRemaining;
    private bool isTripleShotActive;

    private void Start()
    {
        // Ẩn ban đầu cả 2 ô
        if (tripleShotSlot != null) tripleShotSlot.SetActive(false);
        if (shieldSlot != null) shieldSlot.SetActive(false);

        // Đăng ký event từ PlayerPowerUps
        if (playerPowerUps != null)
        {
            playerPowerUps.TripleShotStarted += OnTripleShotStarted;
            playerPowerUps.TripleShotEnded += OnTripleShotEnded;
        }
        else
        {
            Debug.LogWarning("ActiveEffectsHUD: chưa gán playerPowerUps.", this);
        }

        // Đăng ký event từ PlayerHealth và kiểm tra trạng thái ban đầu
        if (playerHealth != null)
        {
            playerHealth.ShieldChanged += OnShieldChanged;
            if (playerHealth.HasShield && shieldSlot != null)
            {
                shieldSlot.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("ActiveEffectsHUD: chưa gán playerHealth.", this);
        }
    }

    private void OnDestroy()
    {
        if (playerPowerUps != null)
        {
            playerPowerUps.TripleShotStarted -= OnTripleShotStarted;
            playerPowerUps.TripleShotEnded -= OnTripleShotEnded;
        }

        if (playerHealth != null)
        {
            playerHealth.ShieldChanged -= OnShieldChanged;
        }
    }

    private void Update()
    {
        if (!isTripleShotActive) return;

        // Đồng hồ nội bộ của UI đếm ngược thời lượng hiển thị
        tripleShotRemaining -= Time.deltaTime;

        if (tripleShotFill != null && tripleShotDuration > 0f)
        {
            tripleShotFill.fillAmount = Mathf.Clamp01(tripleShotRemaining / tripleShotDuration);
        }

        if (tripleShotRemaining <= 0f)
        {
            HideTripleShot();
        }
    }

    private void OnTripleShotStarted(float duration)
    {
        tripleShotDuration = duration;
        tripleShotRemaining = duration;
        isTripleShotActive = true;

        if (tripleShotSlot != null) tripleShotSlot.SetActive(true);
        if (tripleShotFill != null) tripleShotFill.fillAmount = 1f;
    }

    private void OnTripleShotEnded()
    {
        HideTripleShot();
    }

    private void HideTripleShot()
    {
        isTripleShotActive = false;
        tripleShotRemaining = 0f;
        if (tripleShotSlot != null) tripleShotSlot.SetActive(false);
    }

    private void OnShieldChanged(bool hasShield)
    {
        if (shieldSlot != null)
        {
            shieldSlot.SetActive(hasShield);
        }
    }
}

