using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach this to the Screen_HowToPlay GameObject.
/// It builds all visual content at runtime to match the HTML mockup.
/// No Inspector references needed — it finds children by name automatically.
/// </summary>
public class HowToPlayBuilder : MonoBehaviour
{
    // ── Colours matching HTML exactly ──────────────────────────────────────
    static readonly Color C_BG_ORANGE  = Hex("#FFA500");
    static readonly Color C_AMBER      = Hex("#C8860A");
    static readonly Color C_DARK_BROWN = Hex("#7A3800");
    static readonly Color C_MID_BROWN  = Hex("#7A4800");
    static readonly Color C_CREAM      = Hex("#FFFBE6");
    static readonly Color C_WHITE      = Color.white;
    static readonly Color C_GREEN_BG   = Hex("#E8F9E0");
    static readonly Color C_GREEN_BOR  = Hex("#4A9C20");
    static readonly Color C_GREEN_TEXT = Hex("#2E7D0A");
    static readonly Color C_RED_BG     = Hex("#FDECEA");
    static readonly Color C_RED_BOR    = Hex("#C0392B");
    static readonly Color C_RED_TEXT   = Hex("#A82318");

    void Start() => Build();

    void Build()
    {
        // ── Title ──────────────────────────────────────────────────────────
        var titleTMP = GetTMP("TitleText");
        if (titleTMP != null)
        {
            titleTMP.text      = "How to play";
            titleTMP.color     = C_DARK_BROWN;
            titleTMP.fontSize  = 38f;
            titleTMP.fontStyle = FontStyles.Normal;
            titleTMP.alignment = TextAlignmentOptions.Center;
        }

        // ── ScrollView / Content children ─────────────────────────────────
        var scroll  = transform.Find("ScrollView");
        var content = scroll != null ? scroll.Find("Content") : null;

        if (content == null)
        {
            Debug.LogError("[HowToPlayBuilder] Could not find ScrollView/Content — check hierarchy names.");
            return;
        }

        // ── Step Cards ────────────────────────────────────────────────────
        BuildStepCard(content.Find("StepCard1"),
            "Step 1 — Pick a cell type",
            "Press a number key on your keyboard to choose what to place:\n\n" +
            "<color=#C8860A><b>2</b></color> = Pollen   " +
            "<color=#C8860A><b>3</b></color> = Honey   " +
            "<color=#C8860A><b>4</b></color> = Brood   " +
            "<color=#C8860A><b>5</b></color> = Flower   " +
            "<color=#C8860A><b>6</b></color> = Insulation");

        BuildStepCard(content.Find("StepCard2"),
            "Step 2 — Click a hex to paint it",
            "After pressing a number, click any grey hex on the grid. " +
            "It turns that colour. Your bees automatically work on " +
            "whatever cells you place!");

        BuildStepCard(content.Find("StepCard3"),
            "Step 3 — Watch your bees work",
            "Your <b>Forager bee</b> collects Pollen every few seconds.\n" +
            "Your <b>Worker bee</b> turns Pollen into Honey.\n" +
            "More clever patterns = bigger bonuses!");

        // ── Seasons Row ───────────────────────────────────────────────────
        BuildSeasonsRow(content.Find("SeasonsRow"));

        // ── Win / Lose Row ─────────────────────────────────────────────────
        BuildWinLoseRow(content.Find("WinLoseRow"));

        // ── Buttons ────────────────────────────────────────────────────────
        BuildOutlineButton(transform.Find("BtnBack"),      "Back");
        BuildSolidButton  (transform.Find("BtnStartGame"), "Got it — let\u2019s play!");
    }

    // ══════════════════════════════════════════════════════════════════════
    // ── Builders ──────────────────────────────────────────────────────────

    void BuildStepCard(Transform card, string title, string body)
    {
        if (card == null) return;

        // Panel background
        StylePanel(card, C_CREAM, C_AMBER);

        // Find or create title TMP
        var titleT = GetOrCreateTMP(card, "StepTitle");
        titleT.text      = title;
        titleT.color     = C_DARK_BROWN;
        titleT.fontSize  = 15f;
        titleT.fontStyle = FontStyles.Bold;
        titleT.alignment = TextAlignmentOptions.TopLeft;
        FitToTop(titleT.rectTransform, 40f, 14f);

        // Find or create body TMP
        var bodyT = GetOrCreateTMP(card, "StepDesc");
        bodyT.text      = body;
        bodyT.color     = C_MID_BROWN;
        bodyT.fontSize  = 13f;
        bodyT.fontStyle = FontStyles.Normal;
        bodyT.alignment = TextAlignmentOptions.TopLeft;
        FitToBottom(bodyT.rectTransform, 40f, 14f);
    }

    void BuildSeasonsRow(Transform row)
    {
        if (row == null) return;

        string[] icons  = { "🌸", "☀️", "🍂", "❄️" };
        string[] names  = { "Spring", "Summer", "Autumn", "Winter" };
        string[] descs  = {
            "Pollen grows fast.\nBuild your hive!",
            "Max production.\nMake lots of honey!",
            "Pollen slows.\nPlace insulation cells!",
            "Honey drains.\nCan you survive?"
        };

        // Ensure HorizontalLayoutGroup
        var hlg = row.GetComponent<HorizontalLayoutGroup>();
        if (hlg == null) hlg = row.gameObject.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing                  = 10f;
        hlg.childAlignment           = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth    = true;
        hlg.childForceExpandHeight   = false;
        hlg.padding                  = new RectOffset(0, 0, 0, 0);

        // Remove existing children and rebuild
        for (int i = row.childCount - 1; i >= 0; i--)
            DestroyImmediate(row.GetChild(i).gameObject);

        for (int i = 0; i < 4; i++)
        {
            var card = new GameObject($"SeasonCard_{i}", typeof(RectTransform));
            card.transform.SetParent(row, false);
            StylePanel(card.transform, C_CREAM, C_AMBER);

            // Vertical layout inside card
            var vlg = card.AddComponent<VerticalLayoutGroup>();
            vlg.childAlignment         = TextAnchor.UpperCenter;
            vlg.childForceExpandWidth  = true;
            vlg.childForceExpandHeight = false;
            vlg.spacing                = 4f;
            vlg.padding                = new RectOffset(8, 8, 10, 10);

            // Icon
            var iconT = MakeTMP(card.transform, "Icon");
            iconT.text      = icons[i];
            iconT.fontSize  = 22f;
            iconT.color     = C_DARK_BROWN;
            iconT.alignment = TextAlignmentOptions.Center;
            AddLayoutElement(iconT.gameObject, preferredHeight: 30f);

            // Name
            var nameT = MakeTMP(card.transform, "Name");
            nameT.text      = names[i];
            nameT.fontSize  = 14f;
            nameT.fontStyle = FontStyles.Bold;
            nameT.color     = C_DARK_BROWN;
            nameT.alignment = TextAlignmentOptions.Center;
            AddLayoutElement(nameT.gameObject, preferredHeight: 22f);

            // Desc
            var descT = MakeTMP(card.transform, "Desc");
            descT.text      = descs[i];
            descT.fontSize  = 11f;
            descT.color     = C_MID_BROWN;
            descT.alignment = TextAlignmentOptions.Center;
            AddLayoutElement(descT.gameObject, preferredHeight: 36f);

            AddLayoutElement(card, preferredHeight: 110f);
        }
    }

    void BuildWinLoseRow(Transform row)
    {
        if (row == null) return;

        var hlg = row.GetComponent<HorizontalLayoutGroup>();
        if (hlg == null) hlg = row.gameObject.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing                = 12f;
        hlg.childAlignment         = TextAnchor.MiddleCenter;
        hlg.childForceExpandWidth  = true;
        hlg.childForceExpandHeight = false;

        // Remove existing children and rebuild
        for (int i = row.childCount - 1; i >= 0; i--)
            DestroyImmediate(row.GetChild(i).gameObject);

        BuildWinLoseCard(row, "WinCard",
            C_GREEN_BG, C_GREEN_BOR, C_GREEN_TEXT,
            "✓  You WIN if...",
            "Survive winter with 50+ honey left.\nThe bees celebrate!");

        BuildWinLoseCard(row, "LoseCard",
            C_RED_BG, C_RED_BOR, C_RED_TEXT,
            "✗  Game over if...",
            "Your honey runs out in winter.\nThe hive goes cold!");
    }

    void BuildWinLoseCard(Transform parent, string goName,
                          Color bg, Color border, Color textCol,
                          string label, string desc)
    {
        var card = new GameObject(goName, typeof(RectTransform));
        card.transform.SetParent(parent, false);
        StylePanel(card.transform, bg, border);

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment         = TextAnchor.UpperCenter;
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = false;
        vlg.spacing                = 4f;
        vlg.padding                = new RectOffset(12, 12, 12, 12);

        var labelT = MakeTMP(card.transform, "Label");
        labelT.text      = label;
        labelT.color     = textCol;
        labelT.fontSize  = 14f;
        labelT.fontStyle = FontStyles.Bold;
        labelT.alignment = TextAlignmentOptions.Center;
        AddLayoutElement(labelT.gameObject, preferredHeight: 24f);

        var descT = MakeTMP(card.transform, "Desc");
        descT.text      = desc;
        descT.color     = textCol;
        descT.fontSize  = 12f;
        descT.alignment = TextAlignmentOptions.Center;
        AddLayoutElement(descT.gameObject, preferredHeight: 40f);

        AddLayoutElement(card, preferredHeight: 90f);
    }

    void BuildSolidButton(Transform btn, string label)
    {
        if (btn == null) return;
        var img = btn.GetComponent<Image>();
        if (img == null) img = btn.gameObject.AddComponent<Image>();
        img.color = C_AMBER;

        // Rounded look via shadow
        var shadow = btn.GetComponent<Shadow>();
        if (shadow == null) shadow = btn.gameObject.AddComponent<Shadow>();
        shadow.effectColor    = C_DARK_BROWN;
        shadow.effectDistance = new Vector2(0, -2);

        var tmp = GetOrCreateTMP(btn, "Text (TMP)");
        tmp.text      = label;
        tmp.color     = C_WHITE;
        tmp.fontSize  = 18f;
        tmp.fontStyle = FontStyles.Normal;
        tmp.alignment = TextAlignmentOptions.Center;
        StretchFull(tmp.rectTransform);
    }

    void BuildOutlineButton(Transform btn, string label)
    {
        if (btn == null) return;
        var img = btn.GetComponent<Image>();
        if (img == null) img = btn.gameObject.AddComponent<Image>();
        img.color = Color.clear;

        var outline = btn.GetComponent<Outline>();
        if (outline == null) outline = btn.gameObject.AddComponent<Outline>();
        outline.effectColor    = C_AMBER;
        outline.effectDistance = new Vector2(2f, -2f);

        var tmp = GetOrCreateTMP(btn, "Text (TMP)");
        tmp.text      = label;
        tmp.color     = C_MID_BROWN;
        tmp.fontSize  = 16f;
        tmp.fontStyle = FontStyles.Normal;
        tmp.alignment = TextAlignmentOptions.Center;
        StretchFull(tmp.rectTransform);
    }

    // ══════════════════════════════════════════════════════════════════════
    // ── Helpers ───────────────────────────────────────────────────────────

    static void StylePanel(Transform t, Color fill, Color border)
    {
        var img = t.GetComponent<Image>();
        if (img == null) img = t.gameObject.AddComponent<Image>();
        img.color = fill;

        var ol = t.GetComponent<Outline>();
        if (ol == null) ol = t.gameObject.AddComponent<Outline>();
        ol.effectColor    = border;
        ol.effectDistance = new Vector2(2f, -2f);
    }

    TextMeshProUGUI GetTMP(string childName)
    {
        var child = transform.Find(childName);
        return child != null ? child.GetComponent<TextMeshProUGUI>() : null;
    }

    static TextMeshProUGUI GetOrCreateTMP(Transform parent, string name)
    {
        var child = parent.Find(name);
        if (child != null)
        {
            var existing = child.GetComponent<TextMeshProUGUI>();
            if (existing != null) return existing;
        }
        return MakeTMP(parent, name);
    }

    static TextMeshProUGUI MakeTMP(Transform parent, string name)
    {
        var go  = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.enableWordWrapping = true;
        tmp.extraPadding       = true;
        tmp.isRightToLeftText  = false;
        return tmp;
    }

    static void AddLayoutElement(Component c, float preferredHeight = -1f, float preferredWidth = -1f)
        => AddLayoutElement(c.gameObject, preferredHeight, preferredWidth);

    static void AddLayoutElement(GameObject go, float preferredHeight = -1f, float preferredWidth = -1f)
    {
        var le = go.GetComponent<LayoutElement>();
        if (le == null) le = go.AddComponent<LayoutElement>();
        if (preferredHeight >= 0) le.preferredHeight = preferredHeight;
        if (preferredWidth  >= 0) le.preferredWidth  = preferredWidth;
    }

    static void FitToTop(RectTransform rt, float topOffset, float padding)
    {
        rt.anchorMin        = new Vector2(0f, 1f);
        rt.anchorMax        = new Vector2(1f, 1f);
        rt.pivot            = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0f, -padding);
        rt.sizeDelta        = new Vector2(-padding * 2f, topOffset);
    }

    static void FitToBottom(RectTransform rt, float topReserved, float padding)
    {
        rt.anchorMin        = Vector2.zero;
        rt.anchorMax        = new Vector2(1f, 1f);
        rt.offsetMin        = new Vector2(padding, padding);
        rt.offsetMax        = new Vector2(-padding, -(topReserved + padding));
    }

    static void StretchFull(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static Color Hex(string h)
    {
        ColorUtility.TryParseHtmlString(h, out Color c);
        return c;
    }
}