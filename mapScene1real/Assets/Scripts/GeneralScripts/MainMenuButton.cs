using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    private Button button;

    [Header("Scene Settings")]
    [SerializeField] private string homeSceneName = "MainPage";

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnMainMenuClicked);
        }
    }

    void OnMainMenuClicked()
    {
        Debug.Log("Main Menu button clicked - loading homepage...");
        Time.timeScale = 1;
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();
        if (sceneManager != null) sceneManager.LoadMainPage();
        else UnityEngine.SceneManagement.SceneManager.LoadScene("MainPage");
    }
}