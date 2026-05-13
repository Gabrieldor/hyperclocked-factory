using System.Collections.Generic;
using UnityEngine;

/// Manages the item-pipe adjacency graph, BFS routing, and tick-based item transit.
/// Items travel 1 cell per tick; they are invisible in transit (GT style).
public class PipeNetwork : MonoBehaviour
{
    public static PipeNetwork Instance { get; private set; }

    // Delegate for when an item arrives at a machine cell
    public static event System.Action<Vector2Int, ItemData> OnItemArrived;

    private static readonly Vector2Int[] Dirs =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left
    };

    // cell → set of same-color pipe neighbors
    private readonly Dictionary<Vector2Int, HashSet<Vector2Int>> _graph = new();

    // Items in transit: (destination machine cell, item, ticks remaining)
    private readonly List<TransitEntry> _transit = new();

    private struct TransitEntry
    {
        public Vector2Int destination;
        public ItemData item;
        public int ticksLeft;
    }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        TickManager.OnTick += Tick;
        GridManager.OnPipePlaced  += OnPipePlaced;
        GridManager.OnPipeRemoved += OnPipeRemoved;
    }

    private void OnDisable()
    {
        TickManager.OnTick -= Tick;
        GridManager.OnPipePlaced  -= OnPipePlaced;
        GridManager.OnPipeRemoved -= OnPipeRemoved;
    }

    private void OnPipePlaced(PlacedPipe pipe)  => RegisterPipe(pipe);
    private void OnPipeRemoved(PlacedPipe pipe) => UnregisterPipe(pipe.cell);

    // ── Graph maintenance ────────────────────────────────────────────────────

    public void RegisterPipe(PlacedPipe pipe)
    {
        var cell = pipe.cell;
        if (!_graph.ContainsKey(cell)) _graph[cell] = new HashSet<Vector2Int>();

        foreach (var dir in Dirs)
        {
            var neighbor = cell + dir;
            var neighborPipe = GridManager.Instance?.GetPipeAt(neighbor);
            if (neighborPipe == null) continue;
            if (neighborPipe.data.color != pipe.data.color) continue;
            if (neighborPipe.data.layer != pipe.data.layer) continue;

            _graph[cell].Add(neighbor);
            if (_graph.TryGetValue(neighbor, out var nSet))
                nSet.Add(cell);
        }
    }

    public void UnregisterPipe(Vector2Int cell)
    {
        if (!_graph.TryGetValue(cell, out var neighbors)) return;
        foreach (var n in neighbors)
            _graph[n]?.Remove(cell);
        _graph.Remove(cell);
    }

    // ── BFS routing ─────────────────────────────────────────────────────────

    /// Find shortest pipe path from any pipe adjacent to sourceMachineCell (via Output port)
    /// to any pipe adjacent to sinkMachineCell (via Input port).
    /// Returns the list of pipe cells in order, or null if unreachable.
    public List<Vector2Int> FindPath(Vector2Int sourceMachineCell, Vector2Int sinkMachineCell,
                                     PipeColor color, PipeLayer layer)
    {
        // Collect starting pipe cells: pipes adjacent to source with Output port assigned
        var starts = new List<Vector2Int>();
        foreach (var dir in Dirs)
        {
            var adj = sourceMachineCell + dir;
            var pipe = GridManager.Instance?.GetPipeAt(adj);
            if (pipe == null) continue;
            if (pipe.data.color != color || pipe.data.layer != layer) continue;
            if (!pipe.portAssignments.TryGetValue(sourceMachineCell, out var portDir)) continue;
            if (portDir == PortDirection.Output) starts.Add(adj);
        }
        if (starts.Count == 0) return null;

        // Collect goal pipe cells: pipes adjacent to sink with Input port assigned
        var goals = new HashSet<Vector2Int>();
        foreach (var dir in Dirs)
        {
            var adj = sinkMachineCell + dir;
            var pipe = GridManager.Instance?.GetPipeAt(adj);
            if (pipe == null) continue;
            if (pipe.data.color != color || pipe.data.layer != layer) continue;
            if (!pipe.portAssignments.TryGetValue(sinkMachineCell, out var portDir)) continue;
            if (portDir == PortDirection.Input) goals.Add(adj);
        }
        if (goals.Count == 0) return null;

        // BFS
        var queue = new Queue<Vector2Int>();
        var prev = new Dictionary<Vector2Int, Vector2Int>();
        foreach (var s in starts) { queue.Enqueue(s); prev[s] = s; }

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            if (goals.Contains(cur))
                return ReconstructPath(prev, starts, cur);

            if (!_graph.TryGetValue(cur, out var neighbors)) continue;
            foreach (var n in neighbors)
            {
                if (prev.ContainsKey(n)) continue;
                prev[n] = cur;
                queue.Enqueue(n);
            }
        }
        return null;
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> prev,
                                             List<Vector2Int> starts, Vector2Int end)
    {
        var path = new List<Vector2Int>();
        var cur = end;
        while (!starts.Contains(cur))
        {
            path.Add(cur);
            cur = prev[cur];
        }
        path.Add(cur);
        path.Reverse();
        return path;
    }

    // ── Acceptor search ──────────────────────────────────────────────────────

    /// BFS from any pipe adjacent to sourceMachineCell to find the nearest machine
    /// that can accept the given item. Returns (pipe path, destination machine cell).
    /// Returns (null, default) if no accepting machine is reachable.
    public (List<Vector2Int> path, Vector2Int dest) FindPathToAcceptor(Vector2Int sourceMachineCell, ItemData item)
    {
        // Collect start cells: any pipe adjacent to the source machine
        var starts = new List<Vector2Int>();
        foreach (var dir in Dirs)
        {
            var adj = sourceMachineCell + dir;
            if (GridManager.Instance != null && GridManager.Instance.IsPipeAt(adj))
                starts.Add(adj);
        }
        if (starts.Count == 0) return (null, default);

        var queue = new Queue<Vector2Int>();
        var prev  = new Dictionary<Vector2Int, Vector2Int>();

        foreach (var s in starts) { queue.Enqueue(s); prev[s] = s; }

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();

            // Check all cardinal neighbors for an accepting machine
            foreach (var dir in Dirs)
            {
                var neighbor = cur + dir;
                if (neighbor == sourceMachineCell) continue;  // don't loop back to source

                var placed = GridManager.Instance?.GetAt(neighbor);
                if (placed?.instance != null && placed.instance.CanAcceptItem(item))
                {
                    var path = ReconstructPath(prev, starts, cur);
                    return (path, neighbor);
                }
            }

            // Continue BFS through pipes
            if (!_graph.TryGetValue(cur, out var neighbors)) continue;
            foreach (var n in neighbors)
            {
                if (prev.ContainsKey(n)) continue;
                prev[n] = cur;
                queue.Enqueue(n);
            }
        }

        return (null, default);
    }

    // ── Item transit ─────────────────────────────────────────────────────────

    /// Dispatch an item along a pre-found path. Delivery takes path.Count ticks.
    public void DispatchItem(ItemData item, List<Vector2Int> path, Vector2Int destinationMachine)
    {
        _transit.Add(new TransitEntry
        {
            destination = destinationMachine,
            item = item,
            ticksLeft = path.Count
        });
    }

    private void Tick()
    {
        for (int i = _transit.Count - 1; i >= 0; i--)
        {
            var e = _transit[i];
            e.ticksLeft--;
            if (e.ticksLeft <= 0)
            {
                _transit.RemoveAt(i);
                OnItemArrived?.Invoke(e.destination, e.item);
            }
            else
            {
                _transit[i] = e;
            }
        }
    }
}
