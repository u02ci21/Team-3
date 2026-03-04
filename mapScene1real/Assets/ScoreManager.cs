using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text scoreText;
    private static int score = 0;

    void Awake()
    {
        Instance = this;
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