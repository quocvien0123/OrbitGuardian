using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>Menu chính: hiện kỷ lục, mở Credits, vào game, thoát.</summary>
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private string gameSceneName = "Game";

    private void Start()
    {
        // Phòng khi quay về từ màn chơi đang tạm dừng.
        Time.timeScale = 1f;
        Cursor.visible = true;

        if (highScoreText != null)
            highScoreText.text = "KỶ LỤC: " + PlayerPrefs.GetInt("HighScore", 0);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnCreditsClicked()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void OnCloseCreditsClicked()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
