using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public Text scoreText;
    private static int score = 0;

    void Awake()
    {
        Instance = this;
        score = 0;
    }

    public static void AddScore(int amount)
    {
        score += amount;
        if (Instance != null && Instance.scoreText != null)
            Instance.scoreText.text = "Score: " + score;
    }

    public static int GetScore()
    {
        return score;
    }
}