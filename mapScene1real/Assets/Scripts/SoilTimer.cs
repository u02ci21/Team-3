using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = true;

    void Start()
    {
        timeRemaining = timeLimit;
    }

    void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            OnTimeUp();
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // Display as MM:SS
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    void OnTimeUp()
    {
        Debug.Log("Time's up!");
        GameManager.Instance.OnTimeUp();
    }

    public void ApplyPenalty(float seconds)
    {
        timeRemaining -= seconds;
        if (timeRemaining < 0) timeRemaining = 0;
        StartCoroutine(FlashPenalty());
    }

    System.Collections.IEnumerator FlashPenalty()
    {
        timerText.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        timerText.color = Color.white;
    }
}