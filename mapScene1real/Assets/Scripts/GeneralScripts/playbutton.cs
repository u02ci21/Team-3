using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnPlayClicked);
        }
    }

    void OnPlayClicked()
    {
        Debug.Log("Play button clicked - finding GameSceneManager...");

        // Find the GameSceneManager dynamically
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();

        if (sceneManager != null)
        {
            sceneManager.LoadLevelSelect();
        }
        else
        {
            Debug.LogError("GameSceneManager not found!");
        }
    }
}
