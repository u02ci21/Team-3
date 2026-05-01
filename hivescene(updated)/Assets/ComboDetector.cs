using System.Collections.Generic;
using UnityEngine;

public class ComboDetector : MonoBehaviour
{
    public static ComboDetector Instance { get; private set; }

    [Header("Bonus multipliers")]
    public float ringBonus     = 1.5f;   // Ring of 6 → +50% efficiency
    public float spiralBonus   = 2.0f;   // Spiral growth → ×2 production
    public float clusterBonus  = 1.25f;  // 3+ same-type adjacent → +25%

    // Track which coords already claimed a bonus so it doesn't fire repeatedly
    HashSet<Vector2Int> _ringBonusClaimed    = new();
    HashSet<Vector2Int> _clusterBonusClaimed = new();

    public HexGrid hexGrid;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Call this from HexGrid after every placement
    public void CheckCombos(Vector2Int placedCoords)
    {
        CheckRingOfSix(placedCoords);
        CheckCluster(placedCoords);
    }

    // ── Ring of 6: all 6 neighbours of a centre cell are occupied ──────────

    void CheckRingOfSix(Vector2Int placed)
    {
        // Check the placed cell itself as a potential ring centre
        CheckRingAround(placed);

        // Also check each neighbour as a potential ring centre
        foreach (var dir in HexGrid.Directions)
            CheckRingAround(placed + dir);
    }

    void CheckRingAround(Vector2Int centre)
    {
        if (_ringBonusClaimed.Contains(centre)) return;
        if (!hexGrid.Cells.TryGetValue(centre, out _)) return;

        int occupiedNeighbours = 0;
        foreach (var dir in HexGrid.Directions)
        {
            if (hexGrid.Cells.TryGetValue(centre + dir, out HexCell n)
                && n.IsOccupied)
                occupiedNeighbours++;
        }

        if (occupiedNeighbours == 6)
        {
            _ringBonusClaimed.Add(centre);
            TriggerBonus("Ring of 6", centre, ringBonus);
        }
    }

    // ── Cluster: 3+ adjacent cells of the same type ────────────────────────

    void CheckCluster(Vector2Int placed)
    {
        if (!hexGrid.Cells.TryGetValue(placed, out HexCell cell)) return;
        if (!cell.IsOccupied) return;
        if (_clusterBonusClaimed.Contains(placed)) return;

        int count = 1;  // include the placed cell itself
        foreach (var dir in HexGrid.Directions)
        {
            if (hexGrid.Cells.TryGetValue(placed + dir, out HexCell n)
                && n.Type == cell.Type)
                count++;
        }

        if (count >= 3)
        {
            _clusterBonusClaimed.Add(placed);
            TriggerBonus($"{cell.Type} cluster ×{count}", placed, clusterBonus);
        }
    }

    // ── Shared bonus trigger ───────────────────────────────────────────────

    void TriggerBonus(string comboName, Vector2Int coords, float multiplier)
    {
        // Apply bonus to resources
        ResourceManager.Instance?.AddPollen(20f * multiplier);

        // Juice
        Vector3 worldPos = hexGrid.AxialToWorld(coords.x, coords.y);
        JuiceManager.Instance?.PlayComboEffect(worldPos, comboName);
        AudioManager.Instance?.PlayBonusCombo();

        Debug.Log($"[Combo] {comboName} at {coords} → ×{multiplier} bonus!");
    }
}