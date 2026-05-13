using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Panel shown when player taps an existing pipe (no item selected).
/// Displays adjacent machine connections and lets player toggle Input / Output per connection.
public class PipePortPanel : MonoBehaviour
{
    public static PipePortPanel Instance { get; private set; }

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private GameObject entryPrefab;  // has TextMeshProUGUI label + Button toggle

    private Vector2Int _openCell;
    private readonly List<GameObject> _entries = new();

    private static readonly Vector2Int[] Dirs =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left
    };
    private static readonly string[] DirNames = { "North", "South", "East", "West" };

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        if (panel != null) panel.SetActive(false);
    }

    public void Open(Vector2Int pipeCell)
    {
        _openCell = pipeCell;
        var pipe = GridManager.Instance?.GetPipeAt(pipeCell);
        if (pipe == null) return;

        foreach (var e in _entries) Destroy(e);
        _entries.Clear();

        if (titleText != null) titleText.text = $"Pipe Ports [{pipeCell.x},{pipeCell.y}]";

        bool hasAnyMachine = false;
        for (int i = 0; i < 4; i++)
        {
            var neighbor = pipeCell + Dirs[i];
            if (GridManager.Instance?.GetAt(neighbor) == null) continue;
            hasAnyMachine = true;

            if (entryPrefab == null || entryContainer == null) continue;

            var dirName = DirNames[i];
            var machineCell = neighbor;

            var entry = Instantiate(entryPrefab, entryContainer);
            _entries.Add(entry);

            var label = entry.GetComponentInChildren<TextMeshProUGUI>();
            var btn   = entry.GetComponentInChildren<Button>();

            void Refresh()
            {
                pipe.portAssignments.TryGetValue(machineCell, out var dir);
                if (label != null) label.text = $"{dirName}: {dir}";
            }

            if (btn != null)
            {
                btn.onClick.AddListener(() =>
                {
                    pipe.portAssignments.TryGetValue(machineCell, out var dir);
                    pipe.portAssignments[machineCell] =
                        dir == PortDirection.Input ? PortDirection.Output : PortDirection.Input;
                    Refresh();
                });
            }

            Refresh();
        }

        if (panel != null) panel.SetActive(hasAnyMachine);
    }

    public void Close() { if (panel != null) panel.SetActive(false); }
}
