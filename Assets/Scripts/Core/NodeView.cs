using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class NodeView : MonoBehaviour
{
    private NodeInstance _node;
    private NodeManager _manager;
    private SpriteRenderer _sr;

    public void Init(NodeInstance node, NodeManager manager)
    {
        _node = node;
        _manager = manager;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sortingOrder = 0;
        Refresh();
    }

    public void Refresh()
    {
        if (_sr == null) _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = _manager.GetNodeSprite(_node.assignedResource);
    }
}
