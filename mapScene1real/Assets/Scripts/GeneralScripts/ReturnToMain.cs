using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ReturnToMain : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string mainGameSceneName = "harmonygarden";

    [Header("References")]
    [SerializeField] private Button returnButton;

    private bool isLoading = false; // Prevent multiple loads

    void Start()
    {
        if (returnButton == null)
            returnButton = GetComponent<Button>();

        if (returnButton != null)
            returnButton.onClick.AddListener(ReturnToMainGame);
        else
            Debug.LogError("No Button component found on " + gameObject.name);
    }

    void ReturnToMainGame()
    {
        // Prevent spamming the button
        if (isLoading) return;

        Debug.Log("Returning to main game...");
        isLoading = true;

        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();

        if (sceneManager != null)
        {
            sceneManager.LoadMainGame();
        }
        else
        {
            SceneManager.LoadScene(mainGameSceneName);
        }

        // Reset loading flag after a delay
        StartCoroutine(ResetLoadingFlag());
    }

    IEnumerator ResetLoadingFlag()
    {
        yield return new WaitForSeconds(1f);
        isLoading = false;
    }
}