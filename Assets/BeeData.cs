using UnityEngine;

public enum BeeRole { Forager, Worker, Nurse }

[System.Serializable]
public class BeeData
{
    public string Id;
    public BeeRole Role;
    public Vector2Int AssignedCell;   // axial coords of the cell this bee works
    public bool IsActive = true;

    // Per-role tuning
    public float tickRate    = 3f;    // seconds between actions
    public float pollenYield = 5f;    // how much pollen a Forager gathers per tick
    public float convertRate = 4f;    // how much pollen a Worker converts per tick

    public BeeData(string id, BeeRole role, Vector2Int cell)
    {
        Id           = id;
        Role         = role;
        AssignedCell = cell;
    }
}