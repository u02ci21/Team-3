using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HomePageButtonManager : MonoBehaviour
{
    [Header("Button Names and Actions")]
    [SerializeField] private string playButtonName = "PlayButton";
    [SerializeField] private string settingsButtonName = "SettingsButton";
    [SerializeField] private string logoutButtonName = "LogOutButton";
    [SerializeField] private string creditsButtonName = "CreditsButton";
    [SerializeField] private string deleteAccountButtonName = "DeleteAccountButton";
    [SerializeField] private string backButtonName = "BackButton";

    void Start()
    {
        // Give the scene time to load
        StartCoroutine(SetupButtons());
    }

    System.Collections.IEnumerator SetupButtons()
    {
        yield return null; // Wait one frame

        // Find all buttons and reassign them dynamically
        SetupButton(playButtonName, () => OnPlayPressed());
        SetupButton(settingsButtonName, () => OnSettingsPressed());
        SetupButton(logoutButtonName, () => OnLogoutPressed());
        SetupButton(deleteAccountButtonName, () => OnDeleteAccountPressed());
        SetupButton(backButtonName, () => OnBackPressed());

        Debug.Log("All home page buttons reassigned dynamically");
    }

    void SetupButton(string buttonName, UnityEngine.Events.UnityAction action)
    {
        GameObject btnObj = GameObject.Find(buttonName);
        if (btnObj != null)
        {
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(action);
                Debug.Log($"Setup button: {buttonName}");
            }
        }
        else
        {
            Debug.LogWarning($"Button not found: {buttonName}");
        }
    }

    void OnPlayPressed()
    {
        Debug.Log("Play button pressed");
        Time.timeScale = 1;
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();
        if (sceneManager != null) sceneManager.LoadLevelSelect();
        else UnityEngine.SceneManagement.SceneManager.LoadScene("SC All Props");
    }

    void OnSettingsPressed()
    {
        Debug.Log("Settings button pressed");
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();
        if (sceneManager != null) sceneManager.LoadSettingsScene();
    }

    void OnLogoutPressed()
    {
        Debug.Log("Logout button pressed");
        Application.OpenURL("https://team3charlie1.netlify.app/logout.html");
    }

    void OnDeleteAccountPressed()
    {
        Debug.Log("Delete Account button pressed");
        Application.OpenURL("https://team3charlie1.netlify.app/delete.html");
    }

    void OnBackPressed()
    {
        Debug.Log("Back button pressed");
        Time.timeScale = 1;
        GameSceneManager sceneManager = FindFirstObjectByType<GameSceneManager>();
        if (sceneManager != null) sceneManager.LoadMainPage();
        else UnityEngine.SceneManagement.SceneManager.LoadScene("MainPage");
    }

}