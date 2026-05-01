using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public enum Season { Spring, Summer, Autumn, Winter }

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    [Header("Timing")]
    public float secondsPerSeason = 120f;
    public int   totalSeasons     = 8;    // survive 8 seasons (2 full years) to win

    [Header("Sky colours per season")]
    public Color springColor = new Color(0.67f, 0.93f, 0.64f);
    public Color summerColor = new Color(0.53f, 0.81f, 0.98f);
    public Color autumnColor = new Color(0.98f, 0.72f, 0.35f);
    public Color winterColor = new Color(0.78f, 0.88f, 0.97f);

    [Header("Ambient light intensity per season")]
    public float springLight = 1.0f;
    public float summerLight = 1.2f;
    public float autumnLight = 0.85f;
    public float winterLight = 0.6f;

    [Header("Production multipliers per season")]
    public float springPollenMult = 1.2f;
    public float summerPollenMult = 1.5f;
    public float autumnPollenMult = 0.8f;
    public float winterPollenMult = 0.3f;

    public float springHoneyMult = 1.0f;
    public float summerHoneyMult = 1.2f;
    public float autumnHoneyMult = 1.0f;
    public float winterHoneyMult = 0.5f;

    [Header("Win / Lose conditions")]
    public float minHoneyToSurviveWinter = 20f;  // must have this much honey when winter ends

    [Header("References")]
    public Camera  mainCamera;
    public Light2D globalLight;

    // ── Events SeasonUI subscribes to ─────────────────────────────────────
    public UnityEvent<Season> OnSeasonChanged = new();
    public UnityEvent         OnGameOver      = new();
    public UnityEvent         OnGameWin       = new();

    // ── Public read-only state ─────────────────────────────────────────────
    public Season CurrentSeason  { get; private set; } = Season.Spring;
    public float  SeasonTimer    => _timer;                         // SeasonUI reads this
    public float  SeasonProgress => _timer / secondsPerSeason;     // 0-1 for progress bar

    public float PollenMult => CurrentSeason switch
    {
        Season.Spring => springPollenMult,
        Season.Summer => summerPollenMult,
        Season.Autumn => autumnPollenMult,
        Season.Winter => winterPollenMult,
        _             => 1f
    };

    public float HoneyMult => CurrentSeason switch
    {
        Season.Spring => springHoneyMult,
        Season.Summer => summerHoneyMult,
        Season.Autumn => autumnHoneyMult,
        Season.Winter => winterHoneyMult,
        _             => 1f
    };

    float _timer          = 0f;
    bool  _transitioning  = false;
    int   _seasonCount    = 0;    // how many seasons have passed
    bool  _gameOver       = false;

   void Awake()
{
    if (Instance == null) Instance = this;
    else { Destroy(gameObject); return; }

    // Auto-find if not assigned in Inspector
    if (mainCamera == null)
        mainCamera = Camera.main;

    if (globalLight == null)
        globalLight = FindFirstObjectByType<Light2D>();
}

void Start()
{
    ApplySeason(Season.Spring, instant: true);
    OnSeasonChanged.Invoke(Season.Spring);
}

    void Update()
    {
        if (_gameOver) return;

        _timer += Time.deltaTime;
        if (_timer >= secondsPerSeason && !_transitioning)
        {
            _timer = 0f;
            AdvanceSeason();
        }
    }

    // ── Season progression ────────────────────────────────────────────────

    void AdvanceSeason()
    {
        // Check winter survival before moving on
        if (CurrentSeason == Season.Winter)
        {
            if (ResourceManager.Instance != null &&
                ResourceManager.Instance.Honey < minHoneyToSurviveWinter)
            {
                TriggerGameOver();
                return;
            }
        }

        _seasonCount++;

        // Win condition: survived all seasons
        if (_seasonCount >= totalSeasons)
        {
            TriggerWin();
            return;
        }

        Season next = (Season)(((int)CurrentSeason + 1) % 4);
        StartCoroutine(TransitionToSeason(next));
    }

    void TriggerGameOver()
    {
        _gameOver = true;
        Debug.Log("[Season] GAME OVER — not enough honey to survive winter.");
        OnGameOver.Invoke();
    }

    void TriggerWin()
    {
        _gameOver = true;
        Debug.Log("[Season] YOU WIN — hive survived all seasons!");
        OnGameWin.Invoke();
    }

    // ── Visual transition ─────────────────────────────────────────────────

    IEnumerator TransitionToSeason(Season next)
    {
        _transitioning = true;

        Season prev = CurrentSeason;
        CurrentSeason = next;

        // Notify all listeners (SeasonUI, AudioManager, JuiceManager)
        OnSeasonChanged.Invoke(next);
        AudioManager.Instance?.OnSeasonChanged(next);
        JuiceManager.Instance?.PlaySeasonTransition(next);

        Color fromBg    = GetSeasonColor(prev);
        Color toBg      = GetSeasonColor(next);
        float fromLight = GetSeasonLight(prev);
        float toLight   = GetSeasonLight(next);

        float elapsed  = 0f;
        float duration = 3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t      = elapsed / duration;
            float smooth = t * t * (3f - 2f * t);

            if (mainCamera != null)
                mainCamera.backgroundColor = Color.Lerp(fromBg, toBg, smooth);
            if (globalLight != null)
                globalLight.intensity = Mathf.Lerp(fromLight, toLight, smooth);

            yield return null;
        }

        _transitioning = false;
        Debug.Log($"[Season] Now: {next}");
    }

    void ApplySeason(Season s, bool instant = false)
    {
        if (mainCamera != null)  mainCamera.backgroundColor = GetSeasonColor(s);
        if (globalLight != null) globalLight.intensity      = GetSeasonLight(s);
    }

    Color GetSeasonColor(Season s) => s switch
    {
        Season.Spring => springColor,
        Season.Summer => summerColor,
        Season.Autumn => autumnColor,
        Season.Winter => winterColor,
        _             => springColor
    };

    float GetSeasonLight(Season s) => s switch
    {
        Season.Spring => springLight,
        Season.Summer => summerLight,
        Season.Autumn => autumnLight,
        Season.Winter => winterLight,
        _             => 1f
    };
}