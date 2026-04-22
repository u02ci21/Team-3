using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DyslexicFontToggle : MonoBehaviour
{
    public TMP_FontAsset normalFont;
    public TMP_FontAsset dyslexicFont;
    public Toggle toggle;

    private TMP_Text[] allTexts;
    private float[] originalSizes;
    private const float multiplier = 0.55f;

    void Start()
    {
        allTexts = FindObjectsOfType<TMP_Text>();
        originalSizes = new float[allTexts.Length];
        for (int i = 0; i < allTexts.Length; i++)
            originalSizes[i] = allTexts[i].fontSize;

        bool isOn = PlayerPrefs.GetInt("DyslexicFont", 0) == 1;
        
        if (toggle != null)
            toggle.SetIsOnWithoutNotify(isOn);

        OnToggleChanged(isOn);
    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("DyslexicFont", isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        TMP_FontAsset font = isOn ? dyslexicFont : normalFont;
        for (int i = 0; i < allTexts.Length; i++)
        {
            allTexts[i].font = font;
            allTexts[i].fontSize = isOn 
                ? originalSizes[i] * multiplier 
                : originalSizes[i];
        }
    }
}