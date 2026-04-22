using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeasonUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI seasonText;
    public TextMeshProUGUI timerText;
    public Slider          seasonProgressBar;
    public GameObject      gameOverPanel;
    public GameObject      winPanel;

    void Start()
    {
        SeasonManager.Instance.OnSeasonChanged.AddListener(OnSeasonChanged);
        SeasonManager.Instance.OnGameOver.AddListener(ShowGameOver);
        SeasonManager.Instance.OnGameWin.AddListener(ShowWin);

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel      != null) winPanel.SetActive(false);

        UpdateSeasonText(Season.Spring);
    }

    void Update()
    {
        if (SeasonManager.Instance == null) return;

        float remaining = SeasonManager.Instance.secondsPerSeason
                        - SeasonManager.Instance.SeasonTimer;

        if (timerText         != null)
            timerText.text = $"Next: {remaining:F0}s";

        if (seasonProgressBar != null)
            seasonProgressBar.value = SeasonManager.Instance.SeasonProgress;
    }

    void OnSeasonChanged(Season season) => UpdateSeasonText(season);

    void UpdateSeasonText(Season season)
    {
        if (seasonText == null) return;
        seasonText.text = season switch
        {
            Season.Spring => "Spring",
            Season.Summer => "Summer",
            Season.Autumn => "Autumn",
            Season.Winter => "Winter",
            _             => ""
        };
        seasonText.color = season switch
        {
            Season.Spring => new Color(0.4f, 0.9f, 0.4f),
            Season.Summer => new Color(1f,   0.85f, 0.1f),
            Season.Autumn => new Color(0.9f, 0.5f, 0.1f),
            Season.Winter => new Color(0.7f, 0.85f, 1f),
            _             => Color.white
        };
    }

    void ShowGameOver() 
    { 
        if (gameOverPanel != null) gameOverPanel.SetActive(true); 
    }
    
    void ShowWin()      
    { 
        if (winPanel != null) winPanel.SetActive(true); 
    }
}