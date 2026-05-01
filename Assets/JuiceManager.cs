using System.Collections;
using UnityEngine;
using TMPro;

public class JuiceManager : MonoBehaviour
{
    public static JuiceManager Instance { get; private set; }

    [Header("Screen Shake")]
    public float shakeStrength = 0.15f;
    public float shakeDuration = 0.25f;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;  // assign in Inspector (see setup below)

    [Header("Season Transition Flash")]
    public Color springFlash = new Color(0.6f, 1f, 0.5f, 0.4f);
    public Color summerFlash = new Color(1f, 0.95f, 0.3f, 0.4f);
    public Color autumnFlash = new Color(1f, 0.55f, 0.1f, 0.4f);
    public Color winterFlash = new Color(0.7f, 0.9f, 1f, 0.4f);

    Camera _cam;
    Vector3 _camOrigin;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _cam       = Camera.main;
        _camOrigin = _cam.transform.localPosition;
    }

    // ── Called by ComboDetector ────────────────────────────────────────────

    public void PlayComboEffect(Vector3 worldPos, string label)
    {
        StartCoroutine(ShakeCamera());
        SpawnFloatingText(worldPos, label, Color.yellow);
    }

    // ── Called by SeasonManager ────────────────────────────────────────────

    public void PlaySeasonTransition(Season season)
    {
        Color flash = season switch
        {
            Season.Spring => springFlash,
            Season.Summer => summerFlash,
            Season.Autumn => autumnFlash,
            Season.Winter => winterFlash,
            _             => springFlash
        };
        StartCoroutine(FlashScreen(flash));
        SpawnFloatingText(Vector3.zero, season.ToString(), flash);
    }

    // ── Screen shake ───────────────────────────────────────────────────────

    IEnumerator ShakeCamera()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float strength = shakeStrength * (1f - elapsed / shakeDuration);
            _cam.transform.localPosition = _camOrigin + (Vector3)
                (Random.insideUnitCircle * strength);
            yield return null;
        }
        _cam.transform.localPosition = _camOrigin;
    }

    // ── Screen flash ───────────────────────────────────────────────────────

    IEnumerator FlashScreen(Color color)
    {
        // Requires a full-screen UI Image named "FlashPanel" in your Canvas
        // (a simple black Image stretched to fill, with CanvasGroup alpha)
        // If you don't have one yet, this just logs silently
        yield return null;
        Debug.Log($"[Juice] Flash: {color}");
    }

    // ── Floating text ──────────────────────────────────────────────────────

    void SpawnFloatingText(Vector3 worldPos, string text, Color color)
    {
        if (floatingTextPrefab == null) return;

        var go = Instantiate(floatingTextPrefab, worldPos, Quaternion.identity);
        var tmp = go.GetComponentInChildren<TextMeshPro>();
        if (tmp != null)
        {
            tmp.text  = text;
            tmp.color = color;
        }
        StartCoroutine(FloatAndFade(go));
    }

    IEnumerator FloatAndFade(GameObject go)
    {
        var tmp     = go.GetComponentInChildren<TextMeshPro>();
        float elapsed = 0f;
        float duration = 1.5f;
        Vector3 start = go.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            go.transform.position = start + Vector3.up * (t * 1.5f);
            if (tmp != null)
            {
                Color c = tmp.color;
                c.a     = 1f - t;
                tmp.color = c;
            }
            yield return null;
        }
        Destroy(go);
    }
}