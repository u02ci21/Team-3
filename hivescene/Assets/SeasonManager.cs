using UnityEngine;
using UnityEngine.Events;

public enum Season { Spring, Summer, Autumn, Winter }

public class SeasonManager : MonoBehaviour
{
    public static SeasonManager Instance { get; private set; }

    [Header("Timing")]
    public float secondsPerSeason = 120f;

    [Header("Season Multipliers")]
    public float springPollenMult = 1.25f;
    public float summerPollenMult = 1.5f;
    public float summerHoneyMult  = 1.5f;
    public float autumnPollenMult = 0.5f;
    public float winterPollenMult = 0f;      // no foraging in winter

    [Header("Winter Survival")]
    [Tooltip("Each Insulation cell reduces honey drain per tick")]
    public float honeyDrainPerTick      = 2f;
    public float insulationDrainReduce  = 0.4f;  // per insulation cell
    public float winterTickRate         = 5f;     // seconds between drain ticks

    [Header("References")]
    public HexGrid hexGrid;

    // Current state
    public Season CurrentSeason { get; private set; } = Season.Spring;
    public float  SeasonTimer   { get; private set; } = 0f;
    public float  SeasonProgress => SeasonTimer / secondsPerSeason;

    // Multipliers BeeManager reads
    public float PollenMult { get; private set; } = 1f;
    public float HoneyMult  { get; private set; } = 1f;

    // Events UI can subscribe to
    public UnityEvent<Season> OnSeasonChanged = new();
    public UnityEvent         OnGameOver      = new();
    public UnityEvent         OnGameWin       = new();

    bool _gameOver  = false;
    bool _winterDraining = false;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        ApplySeasonMultipliers(Season.Spring);
    }

    void Update()
    {
        if (_gameOver) return;

        SeasonTimer += Time.deltaTime;

        if (SeasonTimer >= secondsPerSeason)
        {
            SeasonTimer = 0f;
            AdvanceSeason();
        }
    }

    // ── Season progression ──────────────────────────────────────────────────

    void AdvanceSeason()
    {
        Season next = CurrentSeason switch
        {
            Season.Spring => Season.Summer,
            Season.Summer => Season.Autumn,
            Season.Autumn => Season.Winter,
            Season.Winter => Season.Spring,
            _             => Season.Spring
        };

        CurrentSeason = next;
        ApplySeasonMultipliers(next);
        OnSeasonChanged.Invoke(next);

        Debug.Log($"[SeasonManager] Season changed to {next}");

        if (next == Season.Winter)
            StartCoroutine(WinterDrainLoop());
    }

    void ApplySeasonMultipliers(Season season)
    {
        switch (season)
        {
            case Season.Spring:
                PollenMult = springPollenMult;
                HoneyMult  = 1f;
                break;
            case Season.Summer:
                PollenMult = summerPollenMult;
                HoneyMult  = summerHoneyMult;
                break;
            case Season.Autumn:
                PollenMult = autumnPollenMult;
                HoneyMult  = 1f;
                break;
            case Season.Winter:
                PollenMult = winterPollenMult;
                HoneyMult  = 1f;
                break;
        }
    }

    // ── Winter drain loop ───────────────────────────────────────────────────

    System.Collections.IEnumerator WinterDrainLoop()
    {
        _winterDraining = true;
        _winterDraining = false;

        while (CurrentSeason == Season.Winter && !_gameOver)
        {
            yield return new UnityEngine.WaitForSeconds(winterTickRate);

            // Count insulation cells
            int insulationCount = 0;
            foreach (var cell in hexGrid.Cells.Values)
                if (cell.Type == CellType.Insulation) insulationCount++;

            float drain = Mathf.Max(0f,
                honeyDrainPerTick - (insulationCount * insulationDrainReduce));

            bool survived = ResourceManager.Instance.SpendHoney(drain);

            Debug.Log($"[Winter] Drained {drain:F1} honey  " +
                      $"(insulation cells: {insulationCount})  " +
                      $"honey left: {ResourceManager.Instance.Honey:F1}");

            if (!survived || ResourceManager.Instance.Honey <= 0f)
            {
                TriggerGameOver();
                yield break;
            }
        }

        // _winterDraining = false;

        // Survived winter — check win condition
        if (CurrentSeason == Season.Spring)
        {
            Debug.Log("[SeasonManager] Survived winter! Spring begins.");

            // Win if honey is above 50 after surviving
            if (ResourceManager.Instance.Honey >= 50f)
            {
                Debug.Log("[SeasonManager] WIN — hive thriving!");
                OnGameWin.Invoke();
            }
        }
    }

    // ── Win / Lose ──────────────────────────────────────────────────────────

    void TriggerGameOver()
    {
        if (_gameOver) return;
        _gameOver = true;
        Debug.Log("[SeasonManager] GAME OVER — hive starved in winter.");
        OnGameOver.Invoke();
    }

    public void ResetGame()
    {
        _gameOver       = false;
        _winterDraining = false;
        SeasonTimer     = 0f;
        CurrentSeason   = Season.Spring;
        ApplySeasonMultipliers(Season.Spring);
    }
}