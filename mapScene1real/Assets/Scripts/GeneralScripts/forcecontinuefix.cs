using UnityEngine;
using UnityEngine.UI;

public class ForceButtonFix : MonoBehaviour
{
    private DialogueSystem dialogueSystem;
    private Button button;

    void Start()
    {
        dialogueSystem = GetComponent<DialogueSystem>();

        // Force find the button
        button = FindObjectOfType<Button>();

        if (button != null && dialogueSystem != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(dialogueSystem.OnContinuePressed);
            Debug.Log("Force button fix applied!");
        }
    }
}