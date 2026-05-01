using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the three tutorial screens:
///   Screen_Welcome   → "How to play" → Screen_HowToPlay
///                    → "Play now"    → Screen_ReadyToPlay
///   Screen_HowToPlay → "Back"        → Screen_Welcome
///                    → "Got it — let's play!" → Screen_ReadyToPlay
///   Screen_ReadyToPlay → "Back"      → Screen_Welcome
///                      → "Start game" → hides TutorialCanvas, shows game Canvas
///
/// Wire-up in Inspector:
///   screenWelcome   = Screen_Welcome  GameObject
///   screenHowToPlay = Screen_HowToPlay GameObject
///   screenReady     = Screen_ReadyToPlay GameObject
///   tutorialCanvas  = TutorialCanvas  GameObject
///   gameCanvas      = Canvas (your main game canvas) GameObject
///   dykText         = the TMP text inside DidYouKnowPanel > DYKText
///
/// Button OnClick events:
///   BtnHowToPlay  → TutorialManager.GoToHowToPlay
///   BtnPlay       → TutorialManager.GoToReady          ← "Play now"
///   BtnBack (HowToPlay screen) → TutorialManager.GoToWelcome
///   BtnStartGame (HowToPlay screen) → TutorialManager.GoToReady  ← "Got it — let's play!"
///   BtnBack (Ready screen)    → TutorialManager.GoToWelcome
///   BtnStartGame (Ready screen) → TutorialManager.StartGame      ← "Start game"
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject screenWelcome;
    public GameObject screenHowToPlay;
    public GameObject screenReady;

    [Header("Did You Know — assign DYKText TMP")]
    public TextMeshProUGUI dykText;

    [Header("Canvases")]
    public GameObject tutorialCanvas;   // the TutorialCanvas root
    public GameObject gameCanvas;       // your main game Canvas root

    // ── Random bee facts ────────────────────────────────────────────────────
    static readonly string[] facts = {
        "Bees visit up to 5,000 flowers just to make one teaspoon of honey!",
        "A honeybee flies at about 25 km per hour — faster than a bicycle!",
        "Bees are responsible for 1 in every 3 bites of food you eat!",
        "A hive can have up to 80,000 bees — that's like a whole city!",
        "Bees do a waggle dance to tell friends where the best flowers are.",
        "Without bees, many fruits and vegetables would disappear.",
        "Bees have five eyes — two big ones and three tiny ones on top!"
    };

    // ── Unity lifecycle ──────────────────────────────────────────────────────
    void Start()
    {
        ValidateReferences();

        // Tutorial visible, game hidden at start
        if (tutorialCanvas != null) tutorialCanvas.SetActive(true);
        if (gameCanvas     != null) gameCanvas.SetActive(false);

        ShowScreen(screenWelcome);
        RefreshFact();
    }

    // ── Public button callbacks ──────────────────────────────────────────────

    /// Called by BtnHowToPlay on Screen_Welcome
    public void GoToHowToPlay() => ShowScreen(screenHowToPlay);

    /// Called by BtnPlay on Screen_Welcome ("Play now")
    /// AND by BtnStartGame on Screen_HowToPlay ("Got it — let's play!")
    public void GoToReady() => ShowScreen(screenReady);

    /// Called by BtnBack on Screen_HowToPlay and Screen_ReadyToPlay
    public void GoToWelcome()
    {
        ShowScreen(screenWelcome);
        RefreshFact();
    }

    /// Called by BtnStartGame on Screen_ReadyToPlay ("Start game")
    public void StartGame()
    {
        if (tutorialCanvas != null) tutorialCanvas.SetActive(false);
        if (gameCanvas     != null) gameCanvas.SetActive(true);

        // Start spring music if AudioManager is present
        if (AudioManager.Instance != null)
            AudioManager.Instance.OnSeasonChanged(Season.Spring);
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    void ShowScreen(GameObject target)
    {
        if (screenWelcome   != null) screenWelcome.SetActive(false);
        if (screenHowToPlay != null) screenHowToPlay.SetActive(false);
        if (screenReady     != null) screenReady.SetActive(false);
        if (target          != null) target.SetActive(true);
    }

    void RefreshFact()
    {
        if (dykText != null)
            dykText.text = facts[Random.Range(0, facts.Length)];
    }

    void ValidateReferences()
    {
        if (screenWelcome   == null) Debug.LogError("[TutorialManager] screenWelcome is not assigned!");
        if (screenHowToPlay == null) Debug.LogError("[TutorialManager] screenHowToPlay is not assigned!");
        if (screenReady     == null) Debug.LogError("[TutorialManager] screenReady is not assigned!");
        if (tutorialCanvas  == null) Debug.LogError("[TutorialManager] tutorialCanvas is not assigned!");
        if (gameCanvas      == null) Debug.LogWarning("[TutorialManager] gameCanvas is not assigned — StartGame() won't show the game.");
        if (dykText         == null) Debug.LogWarning("[TutorialManager] dykText is not assigned — Did You Know won't update.");
    }
}