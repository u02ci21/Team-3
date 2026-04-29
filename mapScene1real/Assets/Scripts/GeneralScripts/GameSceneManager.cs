using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string mainPageSceneName = "MainPage";
    [SerializeField] private string mainGameSceneName = "harmonygarden";
    [SerializeField] private string streamGameSceneName = "streamgame";
    [SerializeField] private string soilGameSceneName = "SoilPlantingScene";
    [SerializeField] private string settingsSceneName = "SettingScene";
    [SerializeField] private string levelSelectSceneName = "SC All Props";

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI percentageText;

    private bool isLoading = false; // Track if a load is in progress

    void Awake()
    {
        // Singleton pattern
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

    // ===== SCENE LOADING METHODS =====

    public void LoadMainPage()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(mainPageSceneName));
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

    public void LoadStreamGame()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(streamGameSceneName));
    }

    public void LoadSoilGame()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(soilGameSceneName));
    }

    public void LoadSettingsScene()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(settingsSceneName));
    }

    public void LoadLevelSelect()
    {
        if (isLoading)
        {
            Debug.Log("Already loading a scene, ignoring request");
            return;
        }
        StartCoroutine(LoadSceneAsync(levelSelectSceneName));
    }

    // ===== ACCOUNT & SETTINGS METHODS =====

    public void OnLogoutPressed()
    {
        Application.OpenURL("https://team3charlie1.netlify.app/logout.html");
    }

    public void OnDeleteAccountPressed()
    {
        Application.OpenURL("https://team3charlie1.netlify.app/delete.html");
    }

    // ===== ASYNC SCENE LOADING COROUTINE =====

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

        // Reset UI elements
        if (progressSlider != null)
            progressSlider.value = 0;
        if (statusText != null)
            statusText.text = "Loading...";
        if (percentageText != null)
            percentageText.text = "0%";

        // Start loading the scene - let it activate naturally
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true; // Allow immediate activation

        // Update progress while loading
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            int percent = Mathf.RoundToInt(progress * 100);

            // Update UI
            if (progressSlider != null)
                progressSlider.value = progress;
            if (percentageText != null)
                percentageText.text = percent + "%";

            yield return null;
        }

        // Hide loading screen
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
        }

        Debug.Log($"Scene {sceneName} fully loaded and activated");
        isLoading = false;
    }
}