using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the factory grid: places and removes machines as GameObjects.
/// Grid origin is (0,0) in world space; each cell is 1 Unity unit.
/// </summary>
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public static event System.Action<PlacedMachine> OnMachinePlaced;
    public static event System.Action<PlacedMachine> OnMachineRemoved;


    [Header("Grid Size")]
    [SerializeField] private int width = 16;
    [SerializeField] private int height = 16;

    [Header("Placeholder Visuals")]
    [SerializeField] private GameObject machinePrefab;  // assign a plain Sprite prefab

    private Dictionary<Vector2Int, PlacedMachine> _grid = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool IsInBounds(Vector2Int cell) =>
        cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;

    public bool IsCellEmpty(Vector2Int cell) => !_grid.ContainsKey(cell);

public bool TryPlace(MachineData data, Vector2Int cell)
    {
        if (!IsInBounds(cell) || !IsCellEmpty(cell)) return false;

        var go = Instantiate(machinePrefab, CellToWorld(cell), Quaternion.identity, transform);
        go.name = $"{data.machineName}_{cell}";

        if (go.TryGetComponent<MachinePlaceholderView>(out var view))
            view.Init(data);

        var placed = new PlacedMachine { data = data, cell = cell, gameObject = go };
        _grid[cell] = placed;
        OnMachinePlaced?.Invoke(placed);
        return true;
    }

public bool TryRemove(Vector2Int cell)
    {
        if (!_grid.TryGetValue(cell, out var placed)) return false;
        Destroy(placed.gameObject);
        _grid.Remove(cell);
        OnMachineRemoved?.Invoke(placed);
        return true;
    }

public bool IsWorkshopPlaced(MachineData workshopData)
    {
        foreach (var m in _grid.Values)
            if (m.data == workshopData) return true;
        return false;
    }


    public PlacedMachine GetAt(Vector2Int cell) =>
        _grid.TryGetValue(cell, out var m) ? m : null;

    public Vector3 CellToWorld(Vector2Int cell) =>
        new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);

    public Vector2Int WorldToCell(Vector3 world) =>
        new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f, 0.3f, 0.3f, 0.4f);
        for (int x = 0; x <= width; x++)
            Gizmos.DrawLine(new Vector3(x, 0), new Vector3(x, height));
        for (int y = 0; y <= height; y++)
            Gizmos.DrawLine(new Vector3(0, y), new Vector3(width, y));
    }
}
