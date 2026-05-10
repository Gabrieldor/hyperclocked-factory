using UnityEngine;

[System.Serializable]
public class NodeInstance
{
    public Vector2Int cell;
    public bool isWaterNode;
    public ItemData assignedResource;
    public bool hasExtractor;
    public NodeView view;
}
