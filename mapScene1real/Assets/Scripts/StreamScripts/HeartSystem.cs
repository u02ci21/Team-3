using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    public static HeartSystem Instance;
    public Image[] hearts;
    private int currentLives;

    void Awake()
    {
        Instance = this;
        currentLives = hearts.Length;
    }

    public static void LoseLife()
    {
        Instance.currentLives--;
        Instance.hearts[Instance.currentLives].enabled = false;

        if (Instance.currentLives <= 0)
            Instance.GameOver();
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        GameManager.Instance.ShowGameOver();
    }
}