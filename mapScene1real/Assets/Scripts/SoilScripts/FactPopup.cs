using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FactPopup : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI factText;
    public Button dismissButton;

    void Start()
    {
        popupPanel.SetActive(false);
    }

    public void ShowFact(string fact)
    {
        factText.text = fact;
        popupPanel.SetActive(true);
        dismissButton.interactable = false;
        Invoke("EnableButton", 0.3f);

        FindObjectOfType<Timer>().StopTimer();
    }

    void EnableButton()
    {
        dismissButton.interactable = true;
    }

    public void DismissPopup()
    {
        StartCoroutine(DismissNextFrame());
    }

    IEnumerator DismissNextFrame()
    {
        yield return null;
        popupPanel.SetActive(false);
        FindObjectOfType<Timer>().ResumeTimer();
        SoilGameManager.Instance.OnFactDismissed();
    }
}