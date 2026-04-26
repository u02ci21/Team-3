using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("Game Progress")]
    [SerializeField] private bool hasCompletedStreamGame = false;
    [SerializeField] private bool hasCompletedSoilGame = false;
    [SerializeField] private int currentDialogueStep = 0;

    [Header("Settings")]
    [SerializeField] private bool usePlayerPrefs = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (usePlayerPrefs)
            {
                LoadProgress();
            }
            else
            {
                ResetAllProgress();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadProgress()
    {
        hasCompletedStreamGame = PlayerPrefs.GetInt("StreamGameCompleted", 0) == 1;
        hasCompletedSoilGame = PlayerPrefs.GetInt("SoilGameCompleted", 0) == 1;
        currentDialogueStep = PlayerPrefs.GetInt("DialogueStep", 0);
        Debug.Log($"Progress loaded: Stream={hasCompletedStreamGame}, Soil={hasCompletedSoilGame}");
    }

    // Stream Game Methods
    public bool HasCompletedStreamGame()
    {
        return hasCompletedStreamGame;
    }

    public void CompleteStreamGame()
    {
        hasCompletedStreamGame = true;
        Debug.Log($"Stream game completed!");

        if (usePlayerPrefs)
        {
            PlayerPrefs.SetInt("StreamGameCompleted", 1);
            PlayerPrefs.Save();
        }
    }

    // Soil Game Methods
    public bool HasCompletedSoilGame()
    {
        return hasCompletedSoilGame;
    }

    public void CompleteSoilGame()
    {
        hasCompletedSoilGame = true;
        Debug.Log($"Soil game completed!");

        if (usePlayerPrefs)
        {
            PlayerPrefs.SetInt("SoilGameCompleted", 1);
            PlayerPrefs.Save();
        }
    }

    // Get overall progress (how many games completed)
    public int GetOverallProgress()
    {
        int completed = 0;
        if (hasCompletedStreamGame) completed++;
        if (hasCompletedSoilGame) completed++;
        return completed;
    }

    // Get total number of games
    public int GetTotalGames()
    {
        return 2; // Update this as you add more games
    }

    public void ResetAllProgress()
    {
        hasCompletedStreamGame = false;
        hasCompletedSoilGame = false;
        currentDialogueStep = 0;

        if (usePlayerPrefs)
        {
            PlayerPrefs.DeleteKey("StreamGameCompleted");
            PlayerPrefs.DeleteKey("SoilGameCompleted");
            PlayerPrefs.DeleteKey("DialogueStep");
            PlayerPrefs.Save();
        }

        Debug.Log("All game progress reset");
    }
}