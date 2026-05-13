using System;
using UnityEngine;

/// Populates PlayerInventory on Start for testing. Remove this GameObject before shipping.
public class DebugStartInventory : MonoBehaviour
{
    [Serializable]
    public class DebugItemEntry
    {
        public ItemData item;
        public int quantity = 64;
    }

    public DebugItemEntry[] startingItems = Array.Empty<DebugItemEntry>();

    private void Start()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null) return;

        foreach (var entry in startingItems)
        {
            if (entry.item == null) continue;
            inv.TryAddItem(entry.item, entry.quantity);
        }
    }
}
