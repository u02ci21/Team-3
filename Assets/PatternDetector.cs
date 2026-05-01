using System.Collections.Generic;
using UnityEngine;

public class PatternDetector : MonoBehaviour
{
    public static PatternDetector Instance { get; private set; }

    // Active multipliers applied each tick
    public float PollenMultiplier { get; private set; } = 1f;
    public float HoneyMultiplier  { get; private set; } = 1f;

    [Header("References")]
    public HexGrid hexGrid;

    [Header("Pattern Multipliers")]
    public float ringOfSixBonus     = 1.5f;
    public float clusterBonus       = 1.25f;   // per 3-cluster
    public float honeyRingBonus     = 2f;
    public float flowerCrownBonus   = 1.75f;

    // Last scan results — BeeManager reads these
    public List<PatternResult> ActivePatterns { get; private set; } = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    // ── Called by HexGrid every time a cell is placed ───────────────────────

    public void ScanAll()
    {
        ActivePatterns.Clear();
        PollenMultiplier = 1f;
        HoneyMultiplier  = 1f;

        foreach (var kvp in hexGrid.Cells)
        {
            var coords = kvp.Key;
            var cell   = kvp.Value;

            if (cell.Type == CellType.Empty) continue;

            CheckRingOfSix(coords);
            CheckSameTypeCluster(coords, cell.Type);
            CheckHoneyRing(coords);
            CheckFlowerCrown(coords);
        }

        // Rebuild multipliers from all active patterns
        foreach (var p in ActivePatterns)
        {
            switch (p.Type)
            {
                case PatternType.RingOfSix:
                case PatternType.SameTypeCluster:
                case PatternType.FlowerCrown:
                    PollenMultiplier *= p.Multiplier;
                    break;
                case PatternType.HoneyRing:
                    HoneyMultiplier *= p.Multiplier;
                    break;
            }
        }

        if (ActivePatterns.Count > 0)
            Debug.Log($"[PatternDetector] {ActivePatterns.Count} pattern(s) active  " +
                      $"pollen×{PollenMultiplier:F2}  honey×{HoneyMultiplier:F2}");
    }

    // ── Ring of Six: all 6 neighbours of a cell are occupied ───────────────

    void CheckRingOfSix(Vector2Int center)
    {
        var neighbors = hexGrid.GetNeighbors(center.x, center.y);
        if (neighbors.Count < 6) return;   // edge cell, can't have 6 neighbours

        foreach (var n in neighbors)
            if (n.Type == CellType.Empty) return;

        // All 6 occupied!
        var cellList = new List<Vector2Int>();
        foreach (var n in neighbors)
            cellList.Add(n.AxialCoords);

        AddIfNew(new PatternResult(
            PatternType.RingOfSix, center, cellList, ringOfSixBonus));
    }

    // ── Same-type cluster: 3+ adjacent cells share the same type ───────────

    void CheckSameTypeCluster(Vector2Int coords, CellType type)
    {
        var neighbors = hexGrid.GetNeighbors(coords.x, coords.y);
        var matching  = new List<Vector2Int> { coords };

        foreach (var n in neighbors)
            if (n.Type == type) matching.Add(n.AxialCoords);

        if (matching.Count < 3) return;

        AddIfNew(new PatternResult(
            PatternType.SameTypeCluster, coords, matching, clusterBonus));
    }

    // ── Honey Ring: all 6 neighbours are HoneyStorage ──────────────────────

    void CheckHoneyRing(Vector2Int center)
    {
        var neighbors = hexGrid.GetNeighbors(center.x, center.y);
        if (neighbors.Count < 6) return;

        foreach (var n in neighbors)
            if (n.Type != CellType.HoneyStorage) return;

        var cellList = new List<Vector2Int>();
        foreach (var n in neighbors)
            cellList.Add(n.AxialCoords);

        AddIfNew(new PatternResult(
            PatternType.HoneyRing, center, cellList, honeyRingBonus));
    }

    // ── Flower Crown: FlowerLink surrounded by 3+ PollenStorage ────────────

    void CheckFlowerCrown(Vector2Int coords)
    {
        if (hexGrid.Cells[coords].Type != CellType.FlowerLink) return;

        var neighbors = hexGrid.GetNeighbors(coords.x, coords.y);
        var pollen    = new List<Vector2Int>();

        foreach (var n in neighbors)
            if (n.Type == CellType.PollenStorage) pollen.Add(n.AxialCoords);

        if (pollen.Count < 3) return;

        pollen.Add(coords);
        AddIfNew(new PatternResult(
            PatternType.FlowerCrown, coords, pollen, flowerCrownBonus));
    }

    // ── Dedup: don't add the same pattern center twice ──────────────────────

    void AddIfNew(PatternResult result)
    {
        foreach (var existing in ActivePatterns)
            if (existing.Type == result.Type && existing.Center == result.Center)
                return;

        ActivePatterns.Add(result);
    }
}