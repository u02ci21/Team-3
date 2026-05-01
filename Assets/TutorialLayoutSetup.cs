using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Applies the full visual style to all three tutorial screens to match the HTML mockup.
/// 
/// HOW TO USE:
///   1. Select the TutorialManager GameObject (or any GameObject in the scene)
///   2. Add this component
///   3. In the Inspector, right-click the component header → "Apply All Screens"
///   4. Remove this component after applying (it is edit-time only)
///
/// The script styles:
///   Screen_Welcome     — title, subtitle, speech bubble, DYK panel, buttons
///   Screen_HowToPlay   — title, step cards, seasons row, win/lose row
///   Screen_ReadyToPlay — speech bubble, reminder panel, buttons
/// </summary>
[ExecuteInEditMode]
public class TutorialLayoutSetup : MonoBehaviour
{
    // ── Colour palette (matching HTML mockup exactly) ──────────────────────
    static readonly Color AMBER      = Hex("#C8860A");
    static readonly Color DARK_BROWN = Hex("#7A3800");
    static readonly Color MID_BROWN  = Hex("#7A4800");
    static readonly Color CREAM      = Hex("#FFFBE6");
    static readonly Color WHITE      = Color.white;
    static readonly Color BTN_BG     = Hex("#C8860A");
    static readonly Color BTN_SHADOW = Hex("#7A4800");

    // ── Screen_Welcome refs ────────────────────────────────────────────────
    [Header("Screen_Welcome")]
    public RectTransform welcomeBackground;
    public TextMeshProUGUI titleTMP;
    public TextMeshProUGUI subtitleTMP;
    public RectTransform   pollieImage;
    public RectTransform   speechBubble;
    public TextMeshProUGUI speechTMP;
    public RectTransform   dykPanel;
    public TextMeshProUGUI dykLabelTMP;
    public TextMeshProUGUI dykTextTMP;
    public RectTransform   btnHowToPlay;
    public TextMeshProUGUI btnHowToPlayTMP;
    public RectTransform   btnPlayNow;
    public TextMeshProUGUI btnPlayNowTMP;

    // ── Screen_HowToPlay refs ──────────────────────────────────────────────
    [Header("Screen_HowToPlay")]
    public TextMeshProUGUI howToTitleTMP;
    public RectTransform   stepCard1;
    public RectTransform   stepCard2;
    public RectTransform   stepCard3;
    public RectTransform   stepCardTip;   // Pro tip card (optional)
    public RectTransform   seasonsRow;    // parent of 4 season cards
    public RectTransform   winLoseRow;    // parent of win/lose cards
    public RectTransform   btnBack_HTP;
    public TextMeshProUGUI btnBackTMP_HTP;
    public RectTransform   btnGotIt;
    public TextMeshProUGUI btnGotItTMP;

    // ── Screen_ReadyToPlay refs ────────────────────────────────────────────
    [Header("Screen_ReadyToPlay")]
    public TextMeshProUGUI readySpeechTMP;
    public RectTransform   readySpeechBubble;
    public RectTransform   reminderPanel;
    public TextMeshProUGUI reminderLabelTMP;
    public TextMeshProUGUI reminderTextTMP;
    public RectTransform   btnBack_RTP;
    public TextMeshProUGUI btnBackTMP_RTP;
    public RectTransform   btnStartGame;
    public TextMeshProUGUI btnStartGameTMP;

    // ══════════════════════════════════════════════════════════════════════
    [ContextMenu("Apply All Screens")]
    public void ApplyAllScreens()
    {
        ApplyWelcomeScreen();
        ApplyHowToPlayScreen();
        ApplyReadyScreen();
        Debug.Log("[TutorialLayoutSetup] All screens styled!");
    }

    // ── WELCOME ────────────────────────────────────────────────────────────
    void ApplyWelcomeScreen()
    {
        // Background — stretch full
        Stretch(welcomeBackground);

        // Title
        if (titleTMP != null)
        {
            StyleText(titleTMP, DARK_BROWN, 52, FontStyles.Normal, TextAlignmentOptions.Center);
            titleTMP.text = "Hive Builder";
            SetAnchored(titleTMP.rectTransform, new Vector2(0.5f,1f), new Vector2(0f,-80f), new Vector2(700f,80f));
        }

        // Subtitle
        if (subtitleTMP != null)
        {
            StyleText(subtitleTMP, MID_BROWN, 28, FontStyles.Normal, TextAlignmentOptions.Center);
            subtitleTMP.text = "A climate puzzle game";
            SetAnchored(subtitleTMP.rectTransform, new Vector2(0.5f,1f), new Vector2(0f,-165f), new Vector2(500f,40f));
        }

        // Pollie image — left center
        if (pollieImage != null)
        {
            pollieImage.anchorMin = new Vector2(0f, 0.5f);
            pollieImage.anchorMax = new Vector2(0f, 0.5f);
            pollieImage.pivot     = new Vector2(0.5f, 0.5f);
            pollieImage.anchoredPosition = new Vector2(190f, 20f);
            pollieImage.sizeDelta        = new Vector2(260f, 310f);
        }

        // Speech bubble
        if (speechBubble != null)
        {
            StylePanel(speechBubble, CREAM, AMBER, 18f);
            speechBubble.anchorMin = new Vector2(0f, 0.5f);
            speechBubble.anchorMax = new Vector2(0f, 0.5f);
            speechBubble.pivot     = new Vector2(0f, 0.5f);
            speechBubble.anchoredPosition = new Vector2(340f, 80f);
            speechBubble.sizeDelta        = new Vector2(540f, 210f);
        }
        if (speechTMP != null)
        {
            StyleText(speechTMP, MID_BROWN, 19f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
            speechTMP.text = "Hi there, young beekeeper! I'm <b>Pollie</b>, your bee guide.\n\n" +
                             "You're going to build a hive, collect pollen, make honey, and " +
                             "survive the cold winter. The planet needs healthy bees — " +
                             "and that means <b>you</b>!";
            StretchPadded(speechTMP.rectTransform, 18f);
        }

        // DYK panel
        if (dykPanel != null)
        {
            StylePanel(dykPanel, CREAM, AMBER, 16f);
            dykPanel.anchorMin = new Vector2(0.5f, 0f);
            dykPanel.anchorMax = new Vector2(0.5f, 0f);
            dykPanel.pivot     = new Vector2(0.5f, 0f);
            dykPanel.anchoredPosition = new Vector2(0f, 140f);
            dykPanel.sizeDelta        = new Vector2(560f, 110f);
        }
        if (dykLabelTMP != null)
        {
            StyleText(dykLabelTMP, AMBER, 13f, FontStyles.Bold, TextAlignmentOptions.TopLeft);
            dykLabelTMP.text = "DID YOU KNOW?";
            dykLabelTMP.characterSpacing = 1.5f;
        }
        if (dykTextTMP != null)
            StyleText(dykTextTMP, MID_BROWN, 15f, FontStyles.Normal, TextAlignmentOptions.TopLeft);

        // Buttons
        StyleButton(btnHowToPlay, btnHowToPlayTMP, "How to play",  BTN_BG, WHITE, 20f);
        StyleButton(btnPlayNow,   btnPlayNowTMP,   "Play now",     BTN_BG, WHITE, 20f);

        if (btnHowToPlay != null)
        {
            btnHowToPlay.anchorMin = new Vector2(0.5f, 0f);
            btnHowToPlay.anchorMax = new Vector2(0.5f, 0f);
            btnHowToPlay.pivot     = new Vector2(0.5f, 0f);
            btnHowToPlay.anchoredPosition = new Vector2(-130f, 55f);
            btnHowToPlay.sizeDelta        = new Vector2(240f, 58f);
        }
        if (btnPlayNow != null)
        {
            btnPlayNow.anchorMin = new Vector2(0.5f, 0f);
            btnPlayNow.anchorMax = new Vector2(0.5f, 0f);
            btnPlayNow.pivot     = new Vector2(0.5f, 0f);
            btnPlayNow.anchoredPosition = new Vector2(130f, 55f);
            btnPlayNow.sizeDelta        = new Vector2(240f, 58f);
        }
    }

    // ── HOW TO PLAY ────────────────────────────────────────────────────────
    void ApplyHowToPlayScreen()
    {
        if (howToTitleTMP != null)
        {
            StyleText(howToTitleTMP, DARK_BROWN, 38f, FontStyles.Normal, TextAlignmentOptions.Center);
            howToTitleTMP.text = "How to play";
        }

        // Step cards
        StyleStepCard(stepCard1,
            "Step 1 — Pick a cell type",
            "Press a number key to choose what to place:\n" +
            "<color=#C8860A>2</color> = Pollen   " +
            "<color=#C8860A>3</color> = Honey   " +
            "<color=#C8860A>4</color> = Brood   " +
            "<color=#C8860A>5</color> = Flower   " +
            "<color=#C8860A>6</color> = Insulation");

        StyleStepCard(stepCard2,
            "Step 2 — Click a hex to paint it",
            "After pressing a number, click any grey hex on the grid. " +
            "It turns that colour. Your bees automatically work on whatever cells you place!");

        StyleStepCard(stepCard3,
            "Step 3 — Watch your bees work",
            "Your <b>Forager bee</b> collects Pollen every few seconds.\n" +
            "Your <b>Worker bee</b> turns Pollen into Honey.\n" +
            "More clever patterns = bigger bonuses!");

        StyleStepCard(stepCardTip,
            "Pro tip — secret patterns!",
            "Surround a cell with <b>6 coloured neighbours</b> → Ring of Six bonus!\n" +
            "Put a Flower cell next to <b>3 Pollen cells</b> → Flower Crown bonus!\n" +
            "Try them both for mega honey!");

        // Season cards — style children
        if (seasonsRow != null)
        {
            string[] icons  = { "🌸", "☀️", "🍂", "❄️" };
            string[] names  = { "Spring", "Summer", "Autumn", "Winter" };
            string[] descs  = {
                "Pollen grows fast.\nBuild your hive!",
                "Max production.\nMake lots of honey!",
                "Pollen slows.\nPlace purple cells!",
                "Honey drains.\nCan you survive?"
            };
            for (int i = 0; i < seasonsRow.childCount && i < 4; i++)
            {
                var card = seasonsRow.GetChild(i) as RectTransform;
                if (card == null) continue;
                StylePanel(card, CREAM, AMBER, 12f);

                // Try to find icon/name/desc TMP children by index
                var tmps = card.GetComponentsInChildren<TextMeshProUGUI>(true);
                if (tmps.Length >= 1) { StyleText(tmps[0], DARK_BROWN, 22f, FontStyles.Normal, TextAlignmentOptions.Center); tmps[0].text = icons[i]; }
                if (tmps.Length >= 2) { StyleText(tmps[1], DARK_BROWN, 14f, FontStyles.Bold,   TextAlignmentOptions.Center); tmps[1].text = names[i]; }
                if (tmps.Length >= 3) { StyleText(tmps[2], MID_BROWN,  11f, FontStyles.Normal, TextAlignmentOptions.Center); tmps[2].text = descs[i]; }
            }
        }

        // Win/Lose row — style children
        if (winLoseRow != null && winLoseRow.childCount >= 2)
        {
            var winCard  = winLoseRow.GetChild(0) as RectTransform;
            var loseCard = winLoseRow.GetChild(1) as RectTransform;

            StylePanel(winCard,  Hex("#E8F9E0"), Hex("#4A9C20"), 12f);
            StylePanel(loseCard, Hex("#FDECEA"), Hex("#C0392B"), 12f);

            var winTmps  = winCard?.GetComponentsInChildren<TextMeshProUGUI>(true);
            var loseTmps = loseCard?.GetComponentsInChildren<TextMeshProUGUI>(true);

            if (winTmps != null && winTmps.Length >= 2)
            {
                StyleText(winTmps[0], Hex("#2E7D0A"), 14f, FontStyles.Bold,   TextAlignmentOptions.Center);
                winTmps[0].text = "✓  You WIN if...";
                StyleText(winTmps[1], Hex("#2E7D0A"), 12f, FontStyles.Normal, TextAlignmentOptions.Center);
                winTmps[1].text = "Survive winter with 50+ honey left. The bees celebrate!";
            }
            if (loseTmps != null && loseTmps.Length >= 2)
            {
                StyleText(loseTmps[0], Hex("#A82318"), 14f, FontStyles.Bold,   TextAlignmentOptions.Center);
                loseTmps[0].text = "✗  Game over if...";
                StyleText(loseTmps[1], Hex("#A82318"), 12f, FontStyles.Normal, TextAlignmentOptions.Center);
                loseTmps[1].text = "Your honey runs out in winter. The hive goes cold!";
            }
        }

        // Buttons
        StyleButton(btnBack_HTP, btnBackTMP_HTP, "Back",              Color.clear, MID_BROWN, 16f, outline: true);
        StyleButton(btnGotIt,    btnGotItTMP,    "Got it — let's play!", BTN_BG,   WHITE,     18f);
    }

    // ── READY TO PLAY ──────────────────────────────────────────────────────
    void ApplyReadyScreen()
    {
        if (readySpeechBubble != null)
            StylePanel(readySpeechBubble, CREAM, AMBER, 18f);

        if (readySpeechTMP != null)
        {
            StyleText(readySpeechTMP, MID_BROWN, 19f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
            readySpeechTMP.text =
                "Ready to save the hive?\n\n" +
                "Remember — <b>Insulation</b> cells are your best friend in winter. " +
                "Place them before it gets cold!";
        }

        if (reminderPanel != null)
            StylePanel(reminderPanel, CREAM, AMBER, 16f);

        if (reminderLabelTMP != null)
        {
            StyleText(reminderLabelTMP, AMBER, 13f, FontStyles.Bold, TextAlignmentOptions.TopLeft);
            reminderLabelTMP.text = "QUICK REMINDER";
            reminderLabelTMP.characterSpacing = 1.5f;
        }

        if (reminderTextTMP != null)
        {
            StyleText(reminderTextTMP, MID_BROWN, 15f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
            reminderTextTMP.text =
                "Press <b><color=#C8860A>2  3  4  5  6</color></b> to pick a cell type, " +
                "then click a hex to place it.\nGood luck!";
        }

        StyleButton(btnBack_RTP,  btnBackTMP_RTP,  "Back",       Color.clear, MID_BROWN, 16f, outline: true);
        StyleButton(btnStartGame, btnStartGameTMP, "Start game", BTN_BG,      WHITE,     18f);
    }

    // ══════════════════════════════════════════════════════════════════════
    // ── Helpers ────────────────────────────────────────────────────────────

    void StyleStepCard(RectTransform card, string title, string body)
    {
        if (card == null) return;
        StylePanel(card, CREAM, AMBER, 14f);

        var tmps = card.GetComponentsInChildren<TextMeshProUGUI>(true);
        if (tmps.Length >= 1)
        {
            StyleText(tmps[0], DARK_BROWN, 15f, FontStyles.Bold, TextAlignmentOptions.TopLeft);
            tmps[0].text = title;
        }
        if (tmps.Length >= 2)
        {
            StyleText(tmps[1], MID_BROWN, 13f, FontStyles.Normal, TextAlignmentOptions.TopLeft);
            tmps[1].text = body;
        }
    }

    void StylePanel(RectTransform rt, Color fill, Color border, float radius)
    {
        if (rt == null) return;
        var img = rt.GetComponent<Image>();
        if (img == null) img = rt.gameObject.AddComponent<Image>();
        img.color = fill;

        // Outline via Unity Outline component (approximates CSS border)
        var outline = rt.GetComponent<Outline>();
        if (outline == null) outline = rt.gameObject.AddComponent<Outline>();
        outline.effectColor    = border;
        outline.effectDistance = new Vector2(2.5f, -2.5f);
        _ = radius; // border-radius isn't settable via script on plain Image; use Sprite with 9-slice instead
    }

    void StyleButton(RectTransform rt, TextMeshProUGUI label,
                     string text, Color bg, Color textColor,
                     float fontSize, bool outline = false)
    {
        if (rt == null) return;

        var img = rt.GetComponent<Image>();
        if (img == null) img = rt.gameObject.AddComponent<Image>();

        if (outline)
        {
            img.color = Color.clear;
            var ol = rt.GetComponent<Outline>();
            if (ol == null) ol = rt.gameObject.AddComponent<Outline>();
            ol.effectColor    = AMBER;
            ol.effectDistance = new Vector2(2f, -2f);
        }
        else
        {
            img.color = bg;
        }

        if (label != null)
        {
            StyleText(label, textColor, fontSize, FontStyles.Normal, TextAlignmentOptions.Center);
            label.text = text;
        }
    }

    void StyleText(TextMeshProUGUI tmp, Color color, float size,
                   FontStyles style, TextAlignmentOptions align)
    {
        if (tmp == null) return;
        tmp.color             = color;
        tmp.fontSize          = size;
        tmp.fontStyle         = style;
        tmp.alignment         = align;
        tmp.isRightToLeftText = false;
        tmp.extraPadding      = true;
        tmp.enableWordWrapping = true;
    }

    void Stretch(RectTransform rt)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void StretchPadded(RectTransform rt, float pad)
    {
        if (rt == null) return;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(pad, pad);
        rt.offsetMax = new Vector2(-pad, -pad);
    }

    void SetAnchored(RectTransform rt, Vector2 anchor, Vector2 pos, Vector2 size)
    {
        if (rt == null) return;
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }

    static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}