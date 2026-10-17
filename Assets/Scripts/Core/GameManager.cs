using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>Quản lý trạng thái lượt chơi, đồng hồ, điểm, kills và kỷ lục.</summary>
public class GameManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    public static GameManager Instance { get; private set; }

    [Tooltip("Thời gian cần sống sót để thắng (giây)")]
    [SerializeField] private float surviveDuration = 180f;
    [Tooltip("Độ trễ từ lúc người chơi chết đến khi hiện Game Over (giây)")]
    [SerializeField] private float gameOverDelay = 1.5f;
    [SerializeField] private int victoryBonus = 1000;
    [SerializeField] private int bonusPerLife = 500;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Debug (chỉ để xem trong Inspector)")]
    [SerializeField] private float elapsedTime;

    public GameState State { get; private set; } = GameState.Playing;
    public int Score { get; private set; }
    public int Kills { get; private set; }
    public float ElapsedTime => elapsedTime;
    public float RemainingTime => Mathf.Max(0f, surviveDuration - elapsedTime);
    public int HighScore { get; private set; }
    public bool IsNewHighScore { get; private set; }

    public event Action<int> ScoreChanged;
    public event Action<int> KillsChanged;
    public event Action<float> TimeChanged;
    public event Action<GameState> StateChanged;

    private bool playerDying;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        SetState(GameState.Playing);
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            TogglePause();

        // Người chơi đã chết (đang chờ Game Over): dừng đồng hồ để không thể thắng nhầm.
        if (State != GameState.Playing || playerDying) return;

        elapsedTime += Time.deltaTime;
        TimeChanged?.Invoke(RemainingTime);
        if (elapsedTime >= surviveDuration)
            WinGame();
    }

    /// <summary>Cộng điểm (chỉ khi đang chơi).</summary>
    public void AddScore(int amount)
    {
        if (State != GameState.Playing) return;
        Score += amount;
        ScoreChanged?.Invoke(Score);
    }

    /// <summary>Tăng số kẻ địch bị hạ (chỉ khi đang chơi).</summary>
    public void AddKill()
    {
        if (State != GameState.Playing) return;
        Kills++;
        KillsChanged?.Invoke(Kills);
    }

    /// <summary>Người chơi hết mạng: sau gameOverDelay giây chuyển sang GameOver.</summary>
    public void OnPlayerDied()
    {
        if (State != GameState.Playing || playerDying) return;
        playerDying = true;
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(gameOverDelay);
        if (State != GameState.Playing) yield break; // đã ở trạng thái khác (an toàn, tránh ghi đè)
        SaveHighScore();
        SetState(GameState.GameOver);
    }

    /// <summary>Tạm dừng / tiếp tục (chỉ giữa Playing và Paused).</summary>
    public void TogglePause()
    {
        if (playerDying) return; // đang chờ Game Over, không cho tạm dừng
        if (State == GameState.Playing)
        {
            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }
        else if (State == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void WinGame()
    {
        int bonus = victoryBonus;
        if (PlayerHealth.Instance != null)
            bonus += bonusPerLife * PlayerHealth.Instance.CurrentLives;
        Score += bonus;
        ScoreChanged?.Invoke(Score);
        SaveHighScore();
        SetState(GameState.Victory);
    }

    private void SaveHighScore()
    {
        IsNewHighScore = Score > HighScore;
        if (!IsNewHighScore) return;
        HighScore = Score;
        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();
    }

    private void SetState(GameState newState)
    {
        State = newState;
        StateChanged?.Invoke(newState);
    }
}
