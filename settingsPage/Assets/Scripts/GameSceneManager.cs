using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    // Add this line - makes it accessible from anywhere
    public static GameSceneManager Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string mainGameSceneName = "harmonygarden";
    [SerializeField] private string streamGameSceneName = "streamgame";

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI percentageText;

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
        else
        {
            Destroy(gameObject);
        }
    }

public void LoadStreamGame()
    {
        StartCoroutine(LoadSceneAsync(streamGameSceneName));
    }

    public void LoadMainGame()
    {
        StartCoroutine(LoadSceneAsync(mainGameSceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("Starting to load scene: " + sceneName);

        // SHOW loading screen
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(true);
            Debug.Log("Loading screen shown");
        }

        // Reset UI elements
        if (progressSlider != null)
            progressSlider.value = 0;

        if (statusText != null)
            statusText.text = "Loading...";

        if (percentageText != null)
            percentageText.text = "0%";

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true; // Let it activate immediately

        // This prevents the loading from completing in the same frame
        asyncLoad.allowSceneActivation = false;

        // Update progress while loading
        while (asyncLoad.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            int percent = Mathf.RoundToInt(progress * 100);

            // Update UI
            if (progressSlider != null)
                progressSlider.value = progress;

            if (statusText != null)
                statusText.text = "Loading...";

            if (percentageText != null)
                percentageText.text = percent + "%";

            Debug.Log("Loading progress: " + percent + "%");
            yield return null;
        }

        // At 90% loaded, scene is almost ready
        Debug.Log("Scene ready to activate!");

        // Update to 100% to show it's ready
        if (progressSlider != null)
            progressSlider.value = 1f;

        if (percentageText != null)
            percentageText.text = "100%";

        if (statusText != null)
            statusText.text = "Loading...";

        // Small delay so player sees 100%
        yield return new WaitForSeconds(0.5f);

        // HIDE loading screen BEFORE activating scene
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
            Debug.Log("Loading screen hidden");
        }

        // Now activate the scene
        asyncLoad.allowSceneActivation = true;
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