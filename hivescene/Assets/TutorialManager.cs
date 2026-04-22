using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject screenWelcome;
    public GameObject screenHowToPlay;
    public GameObject screenReady;

    [Header("Did You Know")]
    public TextMeshProUGUI dykText;

    [Header("Main Game")]
    public GameObject tutorialCanvas;
    public GameObject gameCanvas;

    string[] facts = {
        "Bees visit up to 5,000 flowers just to make one teaspoon of honey!",
        "A honeybee flies at about 25 km per hour — faster than a bicycle!",
        "Bees are responsible for 1 in every 3 bites of food you eat!",
        "A hive can have up to 80,000 bees — that's like a whole city!",
        "Bees do a waggle dance to tell friends where the best flowers are.",
        "Without bees, many fruits and vegetables would disappear.",
        "Bees have five eyes — two big ones and three tiny ones!"
    };

    void Start()
    {
        // Guard: warn loudly if references are missing
        if (screenWelcome == null)   Debug.LogError("TutorialManager: screenWelcome is not assigned!");
        if (screenHowToPlay == null) Debug.LogError("TutorialManager: screenHowToPlay is not assigned!");
        if (screenReady == null)     Debug.LogError("TutorialManager: screenReady is not assigned!");
        if (tutorialCanvas == null)  Debug.LogError("TutorialManager: tutorialCanvas is not assigned!");

        // Make sure the tutorial canvas itself is on
        if (tutorialCanvas != null)
            tutorialCanvas.SetActive(true);

        // Hide game canvas until tutorial is done
        if (gameCanvas != null)
            gameCanvas.SetActive(false);

        ShowScreen(screenWelcome);

        if (dykText != null)
            dykText.text = facts[Random.Range(0, facts.Length)];
    }

    public void GoToHowToPlay() => ShowScreen(screenHowToPlay);
    public void GoToReady()     => ShowScreen(screenReady);

    public void GoToWelcome()
    {
        ShowScreen(screenWelcome);
        if (dykText != null)
            dykText.text = facts[Random.Range(0, facts.Length)];
    }

    public void StartGame()
    {
        if (tutorialCanvas != null) tutorialCanvas.SetActive(false);
        if (gameCanvas != null)     gameCanvas.SetActive(true);
    }

    void ShowScreen(GameObject screen)
    {
        if (screenWelcome != null)   screenWelcome.SetActive(false);
        if (screenHowToPlay != null) screenHowToPlay.SetActive(false);
        if (screenReady != null)     screenReady.SetActive(false);
        if (screen != null)          screen.SetActive(true);
    }
}