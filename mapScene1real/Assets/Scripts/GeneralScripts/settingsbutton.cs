using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSettingsClicked);
        }
    }

    void OnSettingsClicked()
    {
        Debug.Log("Settings button clicked - finding GameSceneManager...");

        // Find the GameSceneManager dynamically
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();

        if (sceneManager != null)
        {
            sceneManager.LoadSettingsScene();
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }
}