using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SoilGameManager : MonoBehaviour
{
    public bool PopupOpen { get; private set; }
    public bool GameOver { get; private set; }
    public static SoilGameManager Instance;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timeRemainingText;

    public bool[] plotCompleted;
    private bool[] factShown;

    [Header("Plots")]
    public SoilPlot[] plots;

    [Header("Facts")]
    private string[] facts = new string[]
    {
        "Seeds contain everything a plant needs to grow - water, sunlight and soil just help wake them up!",
        "Plants help fight climate change by absorbing carbon dioxide from the air as they grow.",
        "Soil needs air, water, and nutrients to help plants grow strong roots.",
        "Crop rotation means planting different crops each season so soil doesn't get tired and worn out.",
        "Biodiversity in soil makes plants stronger and more resistant to disease.",
        "Organic farming doesn't use harmful chemicals, so the soil stays healthy for growing food for years.",
    };

    private int currentPlotIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        plotCompleted = new bool[plots.Length];
        factShown = new bool[plots.Length];
        plots[0].SetLocked(false);
        for (int i = 1; i < plots.Length; i++)
            plots[i].SetLocked(true);
    }

    public void OnPlotCompleted(int plotIndex)
    {
        if (plotCompleted[plotIndex]) return;
        plotCompleted[plotIndex] = true;

        if (!factShown[plotIndex])
        {
            factShown[plotIndex] = true;
            currentPlotIndex = plotIndex;
            PopupOpen = true;
            FactPopup popup = FindObjectOfType<FactPopup>();
            if (popup != null)
                popup.ShowFact(facts[plotIndex]);
        }
        else
        {
            currentPlotIndex = plotIndex;
            UnlockNextPlot();
            CheckWinCondition();
        }
    }

    void UnlockNextPlot()
    {
        int next = currentPlotIndex + 1;
        if (next < plots.Length)
        {
            plots[next].SetLocked(false);
        }
        else
        {
            CheckWinCondition();
        }
    }

    public void OnTimeUp()
    {
        GameOver = true;
        gameOverText.text = "Time's Up!";
        timeRemainingText.text = "";
        gameOverPanel.SetActive(true);
    }

    public void OnFactDismissed()
    {
        PopupOpen = false;
        UnlockNextPlot();
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        bool allComplete = true;
        for (int i = 0; i < plotCompleted.Length; i++)
        {
            if (!plotCompleted[i])
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            GameOver = true;
            FindObjectOfType<Timer>().StopTimer();
            float timeLeft = FindObjectOfType<Timer>().GetTimeRemaining();
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            gameOverText.text = "Well done!";
            timeRemainingText.text = "Time remaining: " + string.Format("{0:00}:{1:00}", minutes, seconds);
            gameOverPanel.SetActive(true);

            ReportCompletionToProgressManager();
        }
    }

    private void ReportCompletionToProgressManager()
    {
        // Find the progress manager and report completion
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.CompleteSoilGame();
            Debug.Log("Soil game completion reported to GameProgressManager!");
        }
        else
        {
            // Fallback to PlayerPrefs
            PlayerPrefs.SetInt("SoilGameCompleted", 1);
            PlayerPrefs.Save();
            Debug.Log("Soil game completion saved to PlayerPrefs");
        }
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainPage");
    }

    public void ReturnToMap()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("harmonygarden");
        // The dialogue will refresh when the scene loads via Start()
    }

    public void ResetPlotCompleted(int plotIndex)
    {
        if (plotIndex >= 0 && plotIndex < plotCompleted.Length)
            plotCompleted[plotIndex] = false;
    }
}