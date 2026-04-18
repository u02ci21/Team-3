using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject levelCompletePanel;

    public int targetScore = 500;

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
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}