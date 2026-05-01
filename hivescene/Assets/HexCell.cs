using UnityEngine;

public enum CellType
{
    Empty,
    PollenStorage,
    HoneyStorage,
    BroodChamber,
    FlowerLink,
    Insulation
}

[System.Serializable]
public class HexCell
{
    public Vector2Int AxialCoords;
    public Vector3 WorldPosition;
    public CellType Type = CellType.Empty;
    
    [Range(0f, 100f)]
    public float FillLevel = 0f;
    
    public string AssignedBeeId = null;   // Phase 3 will use this
    public GameObject Visual;

    public HexCell(int q, int r, Vector3 worldPos)
    {
        AxialCoords = new Vector2Int(q, r);
        WorldPosition = worldPos;
    }

    public bool IsOccupied => Type != CellType.Empty;
    public bool HasBee => !string.IsNullOrEmpty(AssignedBeeId);
}