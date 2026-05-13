using System;
using System.Collections.Generic;
using UnityEngine;

public enum PortDirection { Input, Output }

[Serializable]
public class PlacedPipe
{
    public PipeData data;
    public ItemData sourceItem;   // the hotbar ItemData that was consumed to place this pipe
    public Vector2Int cell;
    public GameObject gameObject;

    // Key = adjacent machine cell, Value = direction relative to that machine
    public Dictionary<Vector2Int, PortDirection> portAssignments = new();
}
