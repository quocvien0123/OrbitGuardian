using TMPro;
using UnityEngine;

/// <summary>Quản lý các panel Tạm dừng / Game Over / Chiến thắng theo GameState.</summary>
public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text gameOverScoreText;
    [SerializeField] private TMP_Text victoryScoreText;
    [SerializeField] private GameObject gameOverNewRecordLabel;
    [SerializeField] private GameObject victoryNewRecordLabel;

    private bool subscribed;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameUI: không tìm thấy GameManager trong scene.", this);
            return;
        }
        GameManager.Instance.StateChanged += OnStateChanged;
        subscribed = true;
        OnStateChanged(GameManager.Instance.State);
    }

    private void OnDestroy()
    {
        if (subscribed && GameManager.Instance != null)
            GameManager.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        SetActive(pausePanel, state == GameState.Paused);
        SetActive(gameOverPanel, state == GameState.GameOver);
        SetActive(victoryPanel, state == GameState.Victory);

        GameManager gm = GameManager.Instance;
        if (state == GameState.GameOver)
        {
            if (gameOverScoreText != null) gameOverScoreText.text = "SCORE: " + gm.Score;
            SetActive(gameOverNewRecordLabel, gm.IsNewHighScore);
        }
        else if (state == GameState.Victory)
        {
            if (victoryScoreText != null) victoryScoreText.text = "SCORE: " + gm.Score;
            SetActive(victoryNewRecordLabel, gm.IsNewHighScore);
        }
    }

    private static void SetActive(GameObject obj, bool active)
    {
        if (obj != null) obj.SetActive(active);
    }

    // Hàm gán cho Button.OnClick
    public void OnResumeClicked()
    {
        // TogglePause chỉ có tác dụng khi đang Paused hoặc Playing.
        if (GameManager.Instance != null && GameManager.Instance.State == GameState.Paused)
            GameManager.Instance.TogglePause();
    }

    public void OnRestartClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.RestartGame();
    }

    public void OnMainMenuClicked()
    {
        if (GameManager.Instance != null) GameManager.Instance.LoadMainMenu();
    }
}
