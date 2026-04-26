using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] private string[] initialDialogue;
    [SerializeField] private string[] afterStreamDialogue;
    [SerializeField] private string[] afterSoilDialogue;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool enableTypewriter = true;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject dialogueBox;

    private int currentLineIndex = 0;
    private string[] currentDialogueSet;
    private bool hasInitialized = false;
    private bool isTyping = false;

    void Start()
    {
        Debug.Log("DialogueSystem Start() called");

        if (continueButton == null)
        {
            continueButton = GetComponentInChildren<Button>();
            if (continueButton == null)
            {
                continueButton = FindObjectOfType<Button>();
            }
        }

        StartCoroutine(InitializeAfterDelay());
    }

    IEnumerator InitializeAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        InitializeDialogue();
        SetupButton();
    }

    void SetupButton()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinuePressed);
            continueButton.interactable = true;
            continueButton.enabled = true;
            continueButton.gameObject.SetActive(true);

            Debug.Log("Continue button setup complete");
        }
        else
        {
            Debug.LogError("Continue button is NULL!");
        }
    }

    void InitializeDialogue()
    {
        if (hasInitialized) return;
        hasInitialized = true;

        bool hasCompletedStream = false;
        bool hasCompletedSoil = false;

        if (GameProgressManager.Instance != null)
        {
            hasCompletedStream = GameProgressManager.Instance.HasCompletedStreamGame();
            hasCompletedSoil = GameProgressManager.Instance.HasCompletedSoilGame();
        }

        Debug.Log($"Progress - Stream: {hasCompletedStream}, Soil: {hasCompletedSoil}");

        if (hasCompletedSoil)
        {
            currentDialogueSet = afterSoilDialogue;
        }
        else if (hasCompletedStream)
        {
            currentDialogueSet = afterStreamDialogue;
        }
        else
        {
            currentDialogueSet = initialDialogue;
        }

        if (currentDialogueSet == null || currentDialogueSet.Length == 0)
        {
            currentDialogueSet = new string[] { "Click continue to start!" };
        }

        currentLineIndex = 0;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
        }

        // Start typewriter for first line
        if (enableTypewriter)
        {
            StartCoroutine(TypewriterEffect(currentDialogueSet[currentLineIndex]));
        }
        else
        {
            dialogueText.text = currentDialogueSet[currentLineIndex];
        }

        SetupButton();
    }

    IEnumerator TypewriterEffect(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in fullText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        Debug.Log($"Finished typing line {currentLineIndex + 1}");
    }

    public void OnContinuePressed()
    {
        // If still typing, stop typing and show full text
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentDialogueSet[currentLineIndex];
            isTyping = false;
            Debug.Log("Skipped to full text");
            return;
        }

        Debug.Log($"Continue clicked - Line {currentLineIndex + 1}/{currentDialogueSet.Length}");

        currentLineIndex++;

        if (currentLineIndex < currentDialogueSet.Length)
        {
            // Show next line with typewriter
            if (enableTypewriter)
            {
                StartCoroutine(TypewriterEffect(currentDialogueSet[currentLineIndex]));
            }
            else
            {
                dialogueText.text = currentDialogueSet[currentLineIndex];
            }
        }
        else
        {
            // End of dialogue
            if (dialogueBox != null)
            {
                dialogueBox.SetActive(false);
            }
            if (continueButton != null)
            {
                continueButton.interactable = false;
            }
        }
    }

    public void RefreshDialogue()
    {
        Debug.Log("Refreshing dialogue...");
        hasInitialized = false;
        currentLineIndex = 0;

        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
        }

        StartCoroutine(InitializeAfterDelay());
    }

    void OnEnable()
    {
        if (hasInitialized)
        {
            SetupButton();
        }
    }
}