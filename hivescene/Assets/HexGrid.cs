using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public float hexSize = 1f;
    public int gridRadius = 3;

    [Header("Visuals")]
    public Sprite hexSprite;
    public Color emptyColor       = new Color(1f, 1f, 1f, 0.3f);
    public Color pollenColor      = new Color(1f, 0.85f, 0.1f);
    public Color honeyColor       = new Color(1f, 0.55f, 0.05f);
    public Color broodColor       = new Color(0.6f, 0.35f, 0.15f);
    public Color flowerLinkColor  = new Color(0.4f, 0.85f, 0.4f);
    public Color insulationColor  = new Color(0.7f, 0.7f, 0.85f);

    [Header("Placement")]
    public CellType selectedType = CellType.PollenStorage;

    public Dictionary<Vector2Int, HexCell> Cells = new();

    static readonly Vector2Int[] Directions = {
        new( 1, 0), new( 1,-1), new( 0,-1),
        new(-1, 0), new(-1, 1), new( 0, 1)
    };

    Camera _cam;
    Keyboard _kb;
    Mouse _mouse;

    void Start()
    {
        _cam   = Camera.main;
        _kb    = Keyboard.current;
        _mouse = Mouse.current;
        GenerateGrid();
    }

    void Update()
    {
        HandleInput();
    }

    // ── Core math ──────────────────────────────────────────────────────────

    public Vector3 AxialToWorld(int q, int r)
    {
        float x = hexSize * 1.5f * q;
        float y = hexSize * Mathf.Sqrt(3f) * (r + q / 2f);
        return new Vector3(x, y, 0f);
    }

    public Vector2Int WorldToAxial(Vector3 worldPos)
    {
        float q = worldPos.x / (hexSize * 1.5f);
        float r = worldPos.y / (hexSize * Mathf.Sqrt(3f)) - q / 2f;
        return RoundAxial(q, r);
    }

    Vector2Int RoundAxial(float qf, float rf)
    {
        float sf = -qf - rf;
        int q = Mathf.RoundToInt(qf);
        int r = Mathf.RoundToInt(rf);
        int s = Mathf.RoundToInt(sf);

        float dq = Mathf.Abs(q - qf);
        float dr = Mathf.Abs(r - rf);
        float ds = Mathf.Abs(s - sf);

        if (dq > dr && dq > ds)  q = -r - s;
        else if (dr > ds)        r = -q - s;

        return new Vector2Int(q, r);
    }

    // ── Grid generation ────────────────────────────────────────────────────

    void GenerateGrid()
    {
        for (int q = -gridRadius; q <= gridRadius; q++)
        {
            int rMin = Mathf.Max(-gridRadius, -q - gridRadius);
            int rMax = Mathf.Min( gridRadius, -q + gridRadius);
            for (int r = rMin; r <= rMax; r++)
                SpawnCell(q, r, CellType.Empty);
        }
    }

    void SpawnCell(int q, int r, CellType type)
    {
        Vector3 worldPos = AxialToWorld(q, r);
        var cell = new HexCell(q, r, worldPos) { Type = type };

        if (hexSprite != null)
        {
            var go = new GameObject($"Hex_{q}_{r}");
            go.transform.SetParent(transform);
            go.transform.position = worldPos;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = hexSprite;
            sr.color  = GetColorForType(type);

            cell.Visual = go;
        }

        Cells[new Vector2Int(q, r)] = cell;
    }

    // ── Input & placement ──────────────────────────────────────────────────

    void HandleInput()
    {
        // Null-check in case no keyboard/mouse is connected
        if (_kb == null) _kb = Keyboard.current;
        if (_mouse == null) _mouse = Mouse.current;
        if (_kb == null || _mouse == null) return;

        // Cycle cell type with number keys 1-6 (New Input System)
        if (_kb.digit1Key.wasPressedThisFrame) selectedType = CellType.Empty;
        if (_kb.digit2Key.wasPressedThisFrame) selectedType = CellType.PollenStorage;
        if (_kb.digit3Key.wasPressedThisFrame) selectedType = CellType.HoneyStorage;
        if (_kb.digit4Key.wasPressedThisFrame) selectedType = CellType.BroodChamber;
        if (_kb.digit5Key.wasPressedThisFrame) selectedType = CellType.FlowerLink;
        if (_kb.digit6Key.wasPressedThisFrame) selectedType = CellType.Insulation;

        // Left click to place (New Input System)
        if (!_mouse.leftButton.wasPressedThisFrame) return;

        Vector2 screenPos = _mouse.position.ReadValue();
        Vector3 worldPos  = _cam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 0f));
        worldPos.z = 0f;

        Vector2Int coords = WorldToAxial(worldPos);

        if (Cells.TryGetValue(coords, out HexCell cell))
            PlaceType(cell);
    }

    void PlaceType(HexCell cell)
{
    cell.Type = selectedType;
    if (cell.Visual != null)
        cell.Visual.GetComponent<SpriteRenderer>().color =
            GetColorForType(selectedType);

    if (PatternDetector.Instance != null) 
        PatternDetector.Instance.ScanAll();
}
    // ── Adjacency helpers ──────────────────────────────────────────────────

    public List<HexCell> GetNeighbors(int q, int r)
    {
        var result = new List<HexCell>();
        foreach (var d in Directions)
            if (Cells.TryGetValue(new Vector2Int(q + d.x, r + d.y), out var n))
                result.Add(n);
        return result;
    }

    public bool HasOccupiedNeighbor(int q, int r)
    {
        foreach (var neighbor in GetNeighbors(q, r))
            if (neighbor.IsOccupied) return true;
        return false;
    }

    // ── Color mapping ──────────────────────────────────────────────────────

    public Color GetColorForType(CellType type) => type switch
    {
        CellType.PollenStorage => pollenColor,
        CellType.HoneyStorage  => honeyColor,
        CellType.BroodChamber  => broodColor,
        CellType.FlowerLink    => flowerLinkColor,
        CellType.Insulation    => insulationColor,
        _                      => emptyColor
    };
}