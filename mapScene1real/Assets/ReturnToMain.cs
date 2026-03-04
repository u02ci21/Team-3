using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ReturnToMain : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string mainGameSceneName = "MainGame";

    [Header("References")]
    [SerializeField] private Button returnButton;

    void Start()
    {
        // Try to get the button component if not assigned
        if (returnButton == null)
            returnButton = GetComponent<Button>();

        // Add a listener to the button
        if (returnButton != null)
            returnButton.onClick.AddListener(ReturnToMainGame);
        else
            Debug.LogError("No Button component found on " + gameObject.name);
    }

    void ReturnToMainGame()
    {
        Debug.Log("Returning to main game...");

        // Try to find the GameSceneManager
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();

        if (sceneManager != null)
        {
            // Use the GameSceneManager to load the main game
            sceneManager.LoadMainGame();
        }
        else
        {
            // Fallback direct load
            Debug.LogWarning("GameSceneManager not found, loading directly");
            SceneManager.LoadScene(mainGameSceneName);
        }
    }
}
