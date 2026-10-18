using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>HUD trong màn chơi: điểm, đồng hồ, kills, mạng (tim đầy/rỗng), chế độ ngắm, thanh hồi chiêu lướt & tự ngắm.</summary>
public class HUDController : MonoBehaviour
{
    [Tooltip("Hiển thị chữ số sprite cho điểm số")]
    [SerializeField] private SpriteNumberDisplay scoreDisplay;

    [Tooltip("Hiển thị chữ số sprite cho số hạ gục")]
    [SerializeField] private SpriteNumberDisplay killsDisplay;

    [Tooltip("Hiển thị chữ số sprite cho số phút")]
    [SerializeField] private SpriteNumberDisplay minutesDisplay;

    [Tooltip("Hiển thị chữ số sprite cho số giây")]
    [SerializeField] private SpriteNumberDisplay secondsDisplay;

    [SerializeField] private TMP_Text aimModeText;
    [SerializeField] private Image dashCooldownFill;
    [SerializeField] private Image autoAimCooldownFill;

    [Tooltip("Icon mạng, theo thứ tự từ trái sang phải")]
    [SerializeField] private Image[] lifeIcons;

    [Tooltip("Sprite trái tim đầy máu")]
    [SerializeField] private Sprite fullHeartSprite;

    [Tooltip("Sprite trái tim rỗng (mất máu)")]
    [SerializeField] private Sprite emptyHeartSprite;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private PlayerMovement playerMovement;

    private int lastShownSeconds = -1;

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        if (gm != null)
        {
            gm.ScoreChanged += OnScoreChanged;
            gm.KillsChanged += OnKillsChanged;
            gm.TimeChanged += OnTimeChanged;
            OnScoreChanged(gm.Score);
            OnKillsChanged(gm.Kills);
            OnTimeChanged(gm.RemainingTime);
        }
        else
        {
            Debug.LogWarning("HUDController: không tìm thấy GameManager trong scene.", this);
        }

        if (playerHealth != null)
        {
            playerHealth.LivesChanged += OnLivesChanged;
            OnLivesChanged(playerHealth.CurrentLives);
        }
        else Debug.LogWarning("HUDController: chưa gán playerHealth.", this);

        if (playerAim != null)
        {
            playerAim.AimModeChanged += OnAimModeChanged;
            OnAimModeChanged(playerAim.IsAutoAim);
        }
        else Debug.LogWarning("HUDController: chưa gán playerAim.", this);

        if (playerMovement == null)
            Debug.LogWarning("HUDController: chưa gán playerMovement.", this);
        if (dashCooldownFill == null)
            Debug.LogWarning("HUDController: chưa gán dashCooldownFill.", this);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ScoreChanged -= OnScoreChanged;
            GameManager.Instance.KillsChanged -= OnKillsChanged;
            GameManager.Instance.TimeChanged -= OnTimeChanged;
        }
        if (playerHealth != null) playerHealth.LivesChanged -= OnLivesChanged;
        if (playerAim != null) playerAim.AimModeChanged -= OnAimModeChanged;
    }

    // Ngoại lệ duy nhất đọc mỗi frame: thanh hồi chiêu lướt và tự ngắm (Q).
    private void Update()
    {
        if (dashCooldownFill != null && playerMovement != null)
            dashCooldownFill.fillAmount = 1f - playerMovement.DashCooldownNormalized;

        if (autoAimCooldownFill != null && playerAim != null)
        {
            if (playerAim.IsAutoAim)
            {
                autoAimCooldownFill.fillAmount = playerAim.AutoAimDuration > 0f
                    ? Mathf.Clamp01(playerAim.AutoAimDurationRemaining / playerAim.AutoAimDuration)
                    : 0f;
            }
            else
            {
                autoAimCooldownFill.fillAmount = 1f - playerAim.AutoAimCooldownNormalized;
            }
        }
    }

    private void OnScoreChanged(int score)
    {
        if (scoreDisplay != null) scoreDisplay.SetValue(score);
    }

    private void OnKillsChanged(int kills)
    {
        if (killsDisplay != null) killsDisplay.SetValue(kills);
    }

    /// <summary>Hiển thị mm và ss, làm tròn lên; chỉ cập nhật khi số giây đổi.</summary>
    private void OnTimeChanged(float remaining)
    {
        int seconds = Mathf.CeilToInt(remaining);
        if (seconds == lastShownSeconds) return;
        lastShownSeconds = seconds;
        if (minutesDisplay != null) minutesDisplay.SetValue(seconds / 60);
        if (secondsDisplay != null) secondsDisplay.SetValue(seconds % 60);
    }

    private void OnLivesChanged(int lives)
    {
        if (lifeIcons == null) return;
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] == null) continue;
            lifeIcons[i].enabled = true;
            Sprite targetSprite = (i < lives) ? fullHeartSprite : emptyHeartSprite;
            if (targetSprite != null) lifeIcons[i].sprite = targetSprite;
        }
    }

    private void OnAimModeChanged(bool isAutoAim)
    {
        if (aimModeText != null)
            aimModeText.text = isAutoAim ? "AIM: AUTO [Q]" : "AIM: MANUAL [Q]";
    }
}
