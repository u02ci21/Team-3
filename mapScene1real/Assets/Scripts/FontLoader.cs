using UnityEngine;
using TMPro;

public class FontLoader : MonoBehaviour
{
    public TMP_FontAsset normalFont;
    public TMP_FontAsset dyslexicFont;
    private const float multiplier = 0.55f;

    void Start()
    {
        bool isOn = PlayerPrefs.GetInt("DyslexicFont", 0) == 1;
        if (!isOn) return;

        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>();
        foreach (TMP_Text text in allTexts)
        {
            text.font = dyslexicFont;
            text.fontSize = text.fontSize * multiplier;
        }
    }
}