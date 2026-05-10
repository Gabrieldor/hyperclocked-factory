using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    [System.Serializable]
    public class NodeDefinition
    {
        public Vector2Int cell;
        public bool isWaterNode;
    }

    [System.Serializable]
    public class NodeResourceEntry
    {
        public ItemData resource;
        public Sprite nodeSprite;
    }

    [Header("Node Layout")]
    [SerializeField] private List<NodeDefinition> nodeDefinitions = new();
    [SerializeField] private GameObject nodePrefab;

    [Header("Resources")]
    [SerializeField] private List<NodeResourceEntry> resourceEntries = new();
    [SerializeField] private List<ItemData> startingResources = new();
    [SerializeField] private ItemData defaultResource;

    private readonly List<NodeInstance> _nodes = new();
    private readonly List<ItemData> _unlockedResources = new();

    public static event System.Action<NodeInstance> OnNodeAssigned;

    public IReadOnlyList<NodeInstance> Nodes => _nodes;
    public IReadOnlyList<ItemData> UnlockedResources => _unlockedResources;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

private void Start()
    {
        foreach (var r in startingResources)
            if (r != null && !_unlockedResources.Contains(r)) _unlockedResources.Add(r);

        foreach (var def in nodeDefinitions)
        {
            var worldPos = GridManager.Instance.CellToWorld(def.cell);
            var go = Instantiate(nodePrefab, worldPos, Quaternion.identity, transform);
            go.name = def.isWaterNode ? "WaterNode" : string.Format("Node_{0}_{1}", def.cell.x, def.cell.y);
            var view = go.GetComponent<NodeView>();
            var instance = new NodeInstance { cell = def.cell, isWaterNode = def.isWaterNode, assignedResource = defaultResource, view = view };
            view.Init(instance, this);
            _nodes.Add(instance);
        }
    }

    public NodeInstance GetNodeAt(Vector2Int cell)
    {
        foreach (var n in _nodes)
            if (n.cell == cell) return n;
        return null;
    }

    public bool IsNodeCell(Vector2Int cell) => GetNodeAt(cell) != null;

    public void TryOpenNodePanel(Vector2Int cell)
    {
        var node = GetNodeAt(cell);
        if (node == null || node.isWaterNode) return;
        NodeSelectionPanel.Instance?.Open(node);
    }

    public void AssignResource(NodeInstance node, ItemData resource)
    {
        node.assignedResource = resource;
        node.view?.Refresh();
        OnNodeAssigned?.Invoke(node);
    }

public Sprite GetNodeSprite(ItemData resource)
    {
        if (resource == null) return null;
        foreach (var e in resourceEntries)
            if (e.resource == resource) return e.nodeSprite;
        return null;
    }

    public void UnlockResource(ItemData resource)
    {
        if (resource != null && !_unlockedResources.Contains(resource))
            _unlockedResources.Add(resource);
    }
}
