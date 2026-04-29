using UnityEngine;
using TMPro;

public class IntroDialogue : MonoBehaviour
{
    public GameObject character;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [TextArea]
    public string message;

    void Start()
    {
        Debug.Log("Dialogue script is running");

        character.SetActive(true);
        dialoguePanel.SetActive(true);
        dialogueText.text = message;
    }


    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        character.SetActive(false);
    }
}
