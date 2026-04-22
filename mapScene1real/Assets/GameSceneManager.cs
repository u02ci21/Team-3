using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string mainGameSceneName = "harmonygarden";
    [SerializeField] private string streamGameSceneName = "streamgame";

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI percentageText;

    private bool isLoading = false; // Track if a load is in progress

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingScreenPanel != null)
                loadingScreenPanel.SetActive(false);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadStreamGame()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(streamGameSceneName));
    }

    public void LoadMainGame()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(mainGameSceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;

        // Don't load the scene we're already in
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.Log($"Already in scene: {sceneName}");
            isLoading = false;
            yield break;
        }

        Debug.Log($"Starting to load scene: {sceneName}");

        // Show loading screen
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(true);
        }

        // Reset UI
        if (progressSlider != null)
            progressSlider.value = 0;
        if (percentageText != null)
            percentageText.text = "0%";
        if (statusText != null)
            statusText.text = "Loading...";

        // Start loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // IMPORTANT: Allow it to activate automatically
        asyncLoad.allowSceneActivation = true;

        // Update progress while loading
        while (!asyncLoad.isDone)
        {
            // Progress goes from 0 to 0.9 for loading, then 1.0 when activated
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            int percent = Mathf.RoundToInt(progress * 100);

            if (progressSlider != null)
                progressSlider.value = progress;
            if (percentageText != null)
                percentageText.text = percent + "%";

            yield return null;
        }

        // Scene is now fully loaded and active
        Debug.Log($"Scene {sceneName} fully loaded");

        // Hide loading screen
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
        }

        isLoading = false;
    }

    public void LoadMainPage()
    {
        StartCoroutine(LoadSceneAsync("MainPage"));
    }

    public void LoadSettingsScene()
    {
        StartCoroutine(LoadSceneAsync("SettingScene"));
    }

    public void LoadSoilGame()
    {
        StartCoroutine(LoadSceneAsync("SoilPlantingScene"));
    }
}