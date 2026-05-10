using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class NodeSelectionPanel : MonoBehaviour
{
    public static NodeSelectionPanel Instance { get; private set; }

    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Button closeButton;

        private CanvasGroup _group;
private NodeInstance _currentNode;
    private readonly List<GameObject> _entries = new();

    public bool IsOpen => _group != null && _group.interactable;

private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        _group = GetComponent<CanvasGroup>();
        if (_group == null) _group = gameObject.AddComponent<CanvasGroup>();
        closeButton.onClick.AddListener(Close);
        Hide();
    }

public void Open(NodeInstance node)
    {
        _currentNode = node;
        RebuildList();
        Show();
    }

public void Close() => Hide();

        private void Hide() { _group.alpha = 0; _group.interactable = false; _group.blocksRaycasts = false; }
    private void Show() { _group.alpha = 1; _group.interactable = true; _group.blocksRaycasts = true; }

private void RebuildList()
    {
        foreach (var e in _entries) Destroy(e);
        _entries.Clear();

        var manager = NodeManager.Instance;
        foreach (var resource in manager.UnlockedResources)
        {
            var go = Instantiate(entryPrefab, entryContainer);
            _entries.Add(go);
            var entry = go.GetComponent<NodeSelectionEntry>();
            entry.Setup(resource, manager.GetNodeSprite(resource), OnEntrySelected);
        }
    }

    private void OnEntrySelected(ItemData resource)
    {
        NodeManager.Instance.AssignResource(_currentNode, resource);
        Close();
    }
}
