using System.Collections.Generic;
using UnityEngine;

public enum PatternType
{
    RingOfSix,        // 6 cells all surrounding one center cell
    SameTypeCluster,  // 3+ adjacent cells of the same type
    HoneyRing,        // Ring of 6 all being HoneyStorage
    FlowerCrown,      // FlowerLink cell surrounded by 3+ PollenStorage
}

[System.Serializable]
public class PatternResult
{
    public PatternType Type;
    public List<Vector2Int> Cells = new();
    public float Multiplier;
    public Vector2Int Center;

    public PatternResult(PatternType type, Vector2Int center, 
                         List<Vector2Int> cells, float multiplier)
    {
        Type       = type;
        Center     = center;
        Cells      = cells;
        Multiplier = multiplier;
    }
}