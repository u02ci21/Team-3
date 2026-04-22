using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    public static BeeManager Instance { get; private set; }

    [Header("References")]
    public HexGrid hexGrid;   // drag your HexGrid GameObject here in Inspector

    [Header("Bee Tuning")]
    [Tooltip("Bonus pollen multiplier per adjacent FlowerLink cell")]
    public float flowerLinkBonus = 0.25f;   // each FlowerLink adds +25% pollen

    [Tooltip("How much pollen one Worker converts to honey per tick")]
    public float workerConvertAmount = 4f;

    [Tooltip("How much honey one Worker deposits into its cell per tick")]
    public float workerFillAmount = 10f;    // added to cell.FillLevel

    [Tooltip("Seconds between Nurse ticks (future use)")]
    public float nurseTick = 10f;

    public List<BeeData> Bees { get; private set; } = new();

    int _nextId = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        // Seed the hive with a small starting colony
        // You'll replace these with real cell coords once the player places cells
        AddBee(BeeRole.Forager, new Vector2Int(0, 0));
        AddBee(BeeRole.Worker,  new Vector2Int(1, 0));
    }

    // ── Public API ──────────────────────────────────────────────────────────

    public BeeData AddBee(BeeRole role, Vector2Int assignedCell)
    {
        var bee = new BeeData($"bee_{_nextId++}", role, assignedCell);
        Bees.Add(bee);
        StartCoroutine(BeeTickLoop(bee));
        Debug.Log($"[BeeManager] Spawned {role} bee → cell {assignedCell}");
        return bee;
    }

    public void RemoveBee(string id)
    {
        Bees.RemoveAll(b => b.Id == id);
        // The coroutine checks IsActive so it will stop itself
    }

    public void AssignBeeToCell(string id, Vector2Int newCell)
    {
        var bee = Bees.Find(b => b.Id == id);
        if (bee != null) bee.AssignedCell = newCell;
    }

    // ── Tick loops ──────────────────────────────────────────────────────────

    IEnumerator BeeTickLoop(BeeData bee)
    {
        while (bee.IsActive)
        {
            yield return new WaitForSeconds(bee.tickRate);

            if (!bee.IsActive) break;

            switch (bee.Role)
            {
                case BeeRole.Forager: ForagerTick(bee); break;
                case BeeRole.Worker:  WorkerTick(bee);  break;
                case BeeRole.Nurse:   NurseTick(bee);   break;
            }
        }
    }

    // ── Forager: gathers pollen, boosted by nearby FlowerLink cells ─────────

    void ForagerTick(BeeData bee)
    {
        if (hexGrid == null) return;

        float bonus = 1f;

        if (hexGrid.Cells.TryGetValue(bee.AssignedCell, out HexCell homeCell))
        {
            // Count adjacent FlowerLink cells and apply bonus
            var neighbors = hexGrid.GetNeighbors(
                bee.AssignedCell.x, bee.AssignedCell.y);

            int flowerCount = 0;
            foreach (var n in neighbors)
                if (n.Type == CellType.FlowerLink) flowerCount++;

            bonus += flowerCount * flowerLinkBonus;
        }

        float pdMult  = PatternDetector.Instance  != null ? PatternDetector.Instance.PollenMultiplier  : 1f;
        float seaMult = SeasonManager.Instance    != null ? SeasonManager.Instance.PollenMult           : 1f;
        float gained  = bee.pollenYield * bonus * pdMult * seaMult;
        ResourceManager.Instance.AddPollen(gained);

        Debug.Log($"[Forager {bee.Id}] +{gained:F1} pollen  " +
                  $"(bonus ×{bonus:F2})  " +
                  $"total={ResourceManager.Instance.Pollen:F1}");
    }

    // ── Worker: converts pollen → honey, fills nearest HoneyStorage cell ────

    void WorkerTick(BeeData bee)
    {
        if (hexGrid == null) return;

        // Try to spend pollen
        bool converted = ResourceManager.Instance.SpendPollen(workerConvertAmount);
        if (!converted)
        {
            Debug.Log($"[Worker {bee.Id}] Not enough pollen to convert.");
            return;
        }

        float honeyMult = (PatternDetector.Instance != null ? PatternDetector.Instance.HoneyMultiplier : 1f)
                * (SeasonManager.Instance   != null ? SeasonManager.Instance.HoneyMult          : 1f);
        ResourceManager.Instance.AddHoney(workerConvertAmount * honeyMult);

        // Fill the assigned cell if it's a HoneyStorage
        if (hexGrid.Cells.TryGetValue(bee.AssignedCell, out HexCell cell)
            && cell.Type == CellType.HoneyStorage)
        {
            cell.FillLevel = Mathf.Clamp(cell.FillLevel + workerFillAmount, 0f, 100f);
            UpdateCellVisual(cell);
        }
        else
        {
            // Find the nearest HoneyStorage neighbour to fill instead
            var neighbors = hexGrid.GetNeighbors(
                bee.AssignedCell.x, bee.AssignedCell.y);

            foreach (var n in neighbors)
            {
                if (n.Type == CellType.HoneyStorage && n.FillLevel < 100f)
                {
                    n.FillLevel = Mathf.Clamp(
                        n.FillLevel + workerFillAmount, 0f, 100f);
                    UpdateCellVisual(n);
                    break;
                }
            }
        }

        Debug.Log($"[Worker {bee.Id}] Converted {workerConvertAmount} pollen → honey  " +
                  $"honey total={ResourceManager.Instance.Honey:F1}");
    }

    // ── Nurse: placeholder for Phase 3b / brood hatching ───────────────────

    void NurseTick(BeeData bee)
    {
        // Phase 4 will grow brood cells into new bees
        Debug.Log($"[Nurse {bee.Id}] tending brood at {bee.AssignedCell}");
    }

    // ── Visual feedback: amber fill darkens as honey fills the cell ─────────

    void UpdateCellVisual(HexCell cell)
    {
        if (cell.Visual == null) return;
        var sr = cell.Visual.GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Lerp from empty-honey colour to a deep amber at 100% fill
        Color empty = hexGrid.GetColorForType(CellType.HoneyStorage);
        Color full  = new Color(0.85f, 0.35f, 0f);   // deep amber
        sr.color    = Color.Lerp(empty, full, cell.FillLevel / 100f);
    }
}