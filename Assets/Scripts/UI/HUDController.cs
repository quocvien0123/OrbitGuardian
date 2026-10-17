using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>HUD trong màn chơi: điểm, đồng hồ, kills, mạng, chế độ ngắm, thanh hồi chiêu lướt.</summary>
public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text aimModeText;
    [SerializeField] private Image dashCooldownFill;
    [Tooltip("Icon mạng, theo thứ tự từ trái sang phải")]
    [SerializeField] private Image[] lifeIcons;
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

    // Ngoại lệ duy nhất đọc mỗi frame: thanh hồi chiêu lướt.
    private void Update()
    {
        if (dashCooldownFill != null && playerMovement != null)
            dashCooldownFill.fillAmount = 1f - playerMovement.DashCooldownNormalized;
    }

    private void OnScoreChanged(int score)
    {
        if (scoreText != null) scoreText.text = "ĐIỂM: " + score;
    }

    private void OnKillsChanged(int kills)
    {
        if (killsText != null) killsText.text = "HẠ: " + kills;
    }

    /// <summary>Hiển thị mm:ss, làm tròn lên; chỉ cập nhật chữ khi số giây đổi.</summary>
    private void OnTimeChanged(float remaining)
    {
        int seconds = Mathf.CeilToInt(remaining);
        if (seconds == lastShownSeconds) return;
        lastShownSeconds = seconds;
        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
    }

    private void OnLivesChanged(int lives)
    {
        if (lifeIcons == null) return;
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null) lifeIcons[i].enabled = i < lives;
        }
    }

    private void OnAimModeChanged(bool isAutoAim)
    {
        if (aimModeText != null)
            aimModeText.text = isAutoAim ? "NGẮM: TỰ ĐỘNG [Q]" : "NGẮM: CHUỘT [Q]";
    }
}
