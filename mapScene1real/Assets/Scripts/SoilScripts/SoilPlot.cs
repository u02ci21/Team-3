using UnityEngine;

public class SoilPlot : MonoBehaviour
{
    public SeedType requiredSeedType;
    public enum PlotState { Untilled, Tilled, Planted, Watered }
    public PlotState currentState = PlotState.Untilled;

    [Header("Sprites")]
    public Sprite untilledSprite;
    public Sprite tilledSprite;

    [Header("Crop")]
    public CropTiler cropTiler;
    public Sprite cropSprite;

    [Header("Watering Timer")]
    public WaterTimerBar waterTimerBar;
    public float waterTimeLimit = 10f;

    private SpriteRenderer spriteRenderer;
    private bool isLocked = true;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private Coroutine waterTimerCoroutine;
    private SeedType plantedSeedType;
    private GameObject plantedSeedObject;

    [SerializeField] private bool isLockedDebug = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = untilledSprite;
        originalColor = Color.white;
    }

    void Start()
    {
        if (waterTimerBar != null)
            waterTimerBar.Hide();
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
        isLockedDebug = locked;

        // Only change visuals if plot is untilled
        if (currentState == PlotState.Untilled)
        {
            originalColor = locked ? new Color(0.6f, 0.6f, 0.6f) : Color.white;
            spriteRenderer.color = originalColor;
            spriteRenderer.sprite = untilledSprite;
        }
    }

    public bool IsLocked() { return isLocked; }
    public bool IsTilled() { return currentState == PlotState.Tilled; }

    void OnMouseDown()
    {
        if (SoilGameManager.Instance.PopupOpen || SoilGameManager.Instance.GameOver) return;

        if (isLocked)
        {
            Debug.Log("This plot is locked!");
            FindObjectOfType<Timer>().ApplyPenalty(5f);
            FlashWrong();
            return;
        }

        WateringCan can = FindObjectOfType<WateringCan>();
        bool canSelected = can != null && can.IsSelected();

        TillTool till = FindObjectOfType<TillTool>();
        bool tillSelected = till != null && till.IsSelected();

        if (currentState == PlotState.Untilled)
        {
            if (canSelected)
            {
                Debug.Log("Till the soil first!");
                FindObjectOfType<Timer>().ApplyPenalty(5f);
                FlashWrong();
                return;
            }
            if (!tillSelected)
            {
                Debug.Log("Select the till tool first!");
                FindObjectOfType<Timer>().ApplyPenalty(5f);
                FlashWrong();
                return;
            }
            Till();
            till.Deselect();
            return;
        }

        if (currentState == PlotState.Tilled)
        {
            if (canSelected)
            {
                Debug.Log("Plant a seed first!");
                FindObjectOfType<Timer>().ApplyPenalty(5f);
                FlashWrong();
            }
            return;
        }

        if (currentState == PlotState.Planted)
        {
            if (canSelected)
            {
                Water();
            }
            else
            {
                Debug.Log("Select the watering can first!");
                FindObjectOfType<Timer>().ApplyPenalty(5f);
                FlashWrong();
            }
            return;
        }

        if (currentState == PlotState.Watered)
        {
            if (canSelected)
            {
                if (waterTimerCoroutine != null)
                    StopCoroutine(waterTimerCoroutine);
                waterTimerCoroutine = StartCoroutine(WaterCountdown());
                if (waterTimerBar != null)
                    waterTimerBar.Show();
                Debug.Log("Re-watered!");
            }
            else
            {
                Debug.Log("Keep watering to maintain the crop!");
            }
        }
    }

    void Till()
    {
        currentState = PlotState.Tilled;
        spriteRenderer.sprite = tilledSprite;
        originalColor = Color.white;
        spriteRenderer.color = originalColor;
        Debug.Log("Soil has been tilled!");
    }

    public bool Plant(SeedType incomingSeedType, GameObject seedObject)
    {
        if (isLocked || currentState != PlotState.Tilled)
        {
            Debug.Log("Can't plant here!");
            return false;
        }

        if (incomingSeedType != requiredSeedType)
        {
            Debug.Log("Wrong seed!");
            return false;
        }

        currentState = PlotState.Planted;
        plantedSeedObject = seedObject;
        originalColor = Color.white;
        spriteRenderer.color = originalColor;

        if (cropTiler != null && cropSprite != null)
            cropTiler.ShowCrop(cropSprite);

        Debug.Log("Seed planted!");
        return true;
    }

    System.Collections.IEnumerator WaterCountdown()
    {
        if (waterTimerBar != null)
            waterTimerBar.Show();

        float elapsed = 0f;
        while (elapsed < waterTimeLimit)
        {
            if (currentState != PlotState.Planted && currentState != PlotState.Watered) yield break;

            if (SoilGameManager.Instance.PopupOpen || SoilGameManager.Instance.GameOver)
            {
                yield return null;
                continue;
            }

            elapsed += Time.deltaTime;
            if (waterTimerBar != null)
                waterTimerBar.SetFill(1f - (elapsed / waterTimeLimit));
            yield return null;
        }

        Debug.Log("Crop dried out!");
        ResetToTilled();
    }

    void ResetToTilled()
    {
        currentState = PlotState.Tilled;
        spriteRenderer.sprite = tilledSprite;
        originalColor = Color.white;
        spriteRenderer.color = originalColor;

        if (cropTiler != null)
            cropTiler.HideCrop();

        if (waterTimerBar != null)
            waterTimerBar.Hide();

        if (plantedSeedObject != null)
        {
            SeedDraggable seed = plantedSeedObject.GetComponent<SeedDraggable>();
            if (seed != null)
                seed.ResetToStart();
            plantedSeedObject.SetActive(true);
            plantedSeedObject = null;
        }

        int plotIndex = System.Array.IndexOf(SoilGameManager.Instance.plots, this);
        SoilGameManager.Instance.ResetPlotCompleted(plotIndex);
    }

    public void FlashWrong()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRed());
    }

    System.Collections.IEnumerator FlashRed()
    {
        Color flashColor = new Color(1f, 0.3f, 0.3f);
        spriteRenderer.color = flashColor;
        if (cropTiler != null) cropTiler.SetColor(flashColor);

        yield return new WaitForSeconds(0.4f);

        spriteRenderer.color = originalColor;
        if (cropTiler != null) cropTiler.SetColor(Color.white);

        flashCoroutine = null;
    }

    void Water()
    {
        if (currentState != PlotState.Planted) return;

        if (waterTimerCoroutine != null)
        {
            StopCoroutine(waterTimerCoroutine);
            waterTimerCoroutine = null;
        }

        currentState = PlotState.Watered;
        originalColor = new Color(0.5f, 0.7f, 1f);
        spriteRenderer.color = originalColor;
        if (cropTiler != null) cropTiler.SetColor(Color.white);

        SoilGameManager.Instance.OnPlotCompleted(System.Array.IndexOf(SoilGameManager.Instance.plots, this));

        waterTimerCoroutine = StartCoroutine(WaterCountdown());

        Debug.Log("Watered!");
    }

    public void Highlight(bool correct)
    {
        if (currentState != PlotState.Tilled) return;
        spriteRenderer.color = correct ? Color.green : Color.white;
    }

    public void ClearHighlight()
    {
        if (currentState == PlotState.Tilled)
        {
            originalColor = Color.white;
            spriteRenderer.color = originalColor;
        }
    }
}