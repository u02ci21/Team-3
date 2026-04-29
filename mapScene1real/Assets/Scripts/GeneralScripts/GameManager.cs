using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;

    public int targetScore = 500;

    private bool hasReportedCompletion = false; // Prevents reporting multiple times

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        hasReportedCompletion = false;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        startPanel.SetActive(false);
    }

    public static void CheckScore(int score)
    {
        if (score >= Instance.targetScore)
            Instance.ShowLevelComplete();
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void ShowLevelComplete()
    {
        Time.timeScale = 0;
        levelCompletePanel.SetActive(true);

        // REPORT COMPLETION TO PROGRESS MANAGER
        ReportCompletionToProgressManager();
    }

    // Report completion to the persistent progress manager
    private void ReportCompletionToProgressManager()
    {
        // Prevent duplicate reports
        if (hasReportedCompletion) return;

        hasReportedCompletion = true;

        // Find the progress manager (it persists across scenes)
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.CompleteStreamGame();
            Debug.Log("Stream game completion reported to GameProgressManager!");
        }
        else
        {
            // Fallback: Use PlayerPrefs if GameProgressManager doesn't exist
            PlayerPrefs.SetInt("StreamGameCompleted", 1);
            PlayerPrefs.Save();
            Debug.Log("Stream game completion saved to PlayerPrefs");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Return to map scene with completion status
    public void ReturnToMap()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("harmonygarden");
        // The dialogue will refresh when the scene loads via Start()
    }
}