using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class TutorialLayoutSetup : MonoBehaviour
{
    [Header("Screen_Welcome Elements")]
    public RectTransform background;
    public RectTransform titleText;
    public RectTransform subtitleText;
    public RectTransform pollieImage;
    public RectTransform speechBubble;
    public RectTransform didYouKnowPanel;
    public RectTransform btnHowToPlay;
    public RectTransform btnPlay;

    [Header("Colors")]
    public Image speechBubbleImage;
    public Image didYouKnowImage;
    public Image btnHowToPlayImage;
    public Image btnPlayImage;

    [Header("Text Colors")]
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI subtitleTMP;
    public TextMeshProUGUI speechTMP;
    public TextMeshProUGUI dykLabelTMP;
    public TextMeshProUGUI dykTextTMP;
    public TextMeshProUGUI btnHowToPlayTMP;
    public TextMeshProUGUI btnPlayTMP;

    [ContextMenu("Apply Layout")]
    public void ApplyLayout()
    {
        Color darkBrown = HexColor("#5C2A00");
        Color amber     = HexColor("#C8860A");
        Color cream     = HexColor("#FFFBE6");
        Color white     = Color.white;

        // Background stretches full screen
        Stretch(background);

        // Title — top center
        SetAnchor(titleText, 0.5f, 1f);
        titleText.anchoredPosition = new Vector2(0, -75);
        titleText.sizeDelta = new Vector2(800, 90);

        // Subtitle — just below title
        SetAnchor(subtitleText, 0.5f, 1f);
        subtitleText.anchoredPosition = new Vector2(0, -165);
        subtitleText.sizeDelta = new Vector2(600, 50);

        // Pollie — middle left, large
        SetAnchor(pollieImage, 0f, 0.5f);
        pollieImage.pivot = new Vector2(0.5f, 0.5f);
        pollieImage.anchoredPosition = new Vector2(200, 30);
        pollieImage.sizeDelta = new Vector2(300, 360);

        // Speech bubble — beside Pollie
        SetAnchor(speechBubble, 0f, 0.5f);
        speechBubble.pivot = new Vector2(0f, 0.5f);
        speechBubble.anchoredPosition = new Vector2(370, 100);
        speechBubble.sizeDelta = new Vector2(580, 220);

        // Did You Know — below speech bubble
        SetAnchor(didYouKnowPanel, 0f, 0.5f);
        didYouKnowPanel.pivot = new Vector2(0f, 0.5f);
        didYouKnowPanel.anchoredPosition = new Vector2(370, -110);
        didYouKnowPanel.sizeDelta = new Vector2(580, 140);

        // Buttons — bottom center
        SetAnchor(btnHowToPlay, 0.5f, 0f);
        btnHowToPlay.pivot = new Vector2(0.5f, 0f);
        btnHowToPlay.anchoredPosition = new Vector2(-155, 60);
        btnHowToPlay.sizeDelta = new Vector2(260, 65);

        SetAnchor(btnPlay, 0.5f, 0f);
        btnPlay.pivot = new Vector2(0.5f, 0f);
        btnPlay.anchoredPosition = new Vector2(155, 60);
        btnPlay.sizeDelta = new Vector2(260, 65);

        // Panel colors
        SetColor(speechBubbleImage,  cream);
        SetColor(didYouKnowImage,    cream);
        SetColor(btnHowToPlayImage,  HexColor("#8B6914"));
        SetColor(btnPlayImage,       HexColor("#8B6914"));

        // Outlines
        AddOutline(speechBubble.gameObject,    HexColor("#D4A017"));
        AddOutline(didYouKnowPanel.gameObject, HexColor("#D4A017"));

        // Text styles — RTL OFF on all
        ApplyText(titleTMP,        darkBrown, 75,
            FontStyles.Normal, TextAlignmentOptions.Center);
        ApplyText(subtitleTMP,     HexColor("#A06010"), 35,
            FontStyles.Normal, TextAlignmentOptions.Center);
        ApplyText(speechTMP,       darkBrown, 22,
            FontStyles.Normal, TextAlignmentOptions.Left);
        ApplyText(dykLabelTMP,     amber,     17,
            FontStyles.Bold,   TextAlignmentOptions.Left);
        ApplyText(dykTextTMP,      darkBrown, 22,
            FontStyles.Normal, TextAlignmentOptions.Left);
        ApplyText(btnHowToPlayTMP, white,     24,
            FontStyles.Normal, TextAlignmentOptions.Center);
        ApplyText(btnPlayTMP,      white,     24,
            FontStyles.Normal, TextAlignmentOptions.Center);

        // Text content
        if (titleTMP    != null) titleTMP.text    = "Hive Builder";
        if (subtitleTMP != null) subtitleTMP.text = "A climate puzzle game";
        if (speechTMP   != null)
        {
            speechTMP.text = "Hi there, young beekeeper! I'm <b>Pollie</b>," +
                " your bee guide.\n\nYou're going to build a hive, collect" +
                " pollen, make honey, and survive the cold winter. The planet" +
                " needs healthy bees — and that means <b>you</b>!";
            speechTMP.isRightToLeftText = false;
        }
        if (dykLabelTMP != null)
        {
            dykLabelTMP.text = "DID YOU KNOW?";
            dykLabelTMP.isRightToLeftText = false;
        }
        if (dykTextTMP != null)
            dykTextTMP.isRightToLeftText = false;
        if (btnHowToPlayTMP != null)
        {
            btnHowToPlayTMP.text = "How to play";
            btnHowToPlayTMP.isRightToLeftText = false;
        }
        if (btnPlayTMP != null)
        {
            btnPlayTMP.text = "Play now";
            btnPlayTMP.isRightToLeftText = false;
        }

        // Stretch SpeechText inside bubble
        if (speechTMP != null)
            StretchInside(speechTMP.GetComponent<RectTransform>(), 20);

        // DYKLabel top left inside panel
        if (dykLabelTMP != null)
        {
            var rt = dykLabelTMP.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.anchoredPosition = new Vector2(16, -12);
            rt.sizeDelta = new Vector2(300, 28);
        }

        // DYKText fills rest of panel
        if (dykTextTMP != null)
        {
            var rt = dykTextTMP.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.offsetMin = new Vector2(16, 10);
            rt.offsetMax = new Vector2(-16, -44);
        }

        Debug.Log("[TutorialLayoutSetup] Layout applied!");
    }

    void Stretch(RectTransform rt)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void StretchInside(RectTransform rt, float padding)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(padding, padding);
        rt.offsetMax = new Vector2(-padding, -padding);
    }

    void SetAnchor(RectTransform rt, float x, float y)
    {
        if (rt == null) return;
        rt.anchorMin = new Vector2(x, y);
        rt.anchorMax = new Vector2(x, y);
    }

    void SetColor(Image img, Color color)
    {
        if (img != null) img.color = color;
    }

    void ApplyText(TextMeshProUGUI tmp, Color color,
                   float size, FontStyles style,
                   TextAlignmentOptions align)
    {
        if (tmp == null) return;
        tmp.color               = color;
        tmp.fontSize            = size;
        tmp.fontStyle           = style;
        tmp.alignment           = align;
        tmp.isRightToLeftText   = false;
        tmp.extraPadding        = true;
    }

    void AddOutline(GameObject go, Color color)
    {
        if (go == null) return;
        var o = go.GetComponent<Outline>();
        if (o == null) o = go.AddComponent<Outline>();
        o.effectColor    = color;
        o.effectDistance = new Vector2(2, -2);
    }

    Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}