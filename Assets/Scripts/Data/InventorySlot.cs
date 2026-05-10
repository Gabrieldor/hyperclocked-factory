using System;
using UnityEngine;

/// Runtime inventory slot. One item type + quantity.
/// Not a ScriptableObject — lives only in memory, serialised as part of GridState save (Phase 2).
[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Set(ItemData newItem, int qty) { item = newItem; quantity = qty; }
    public void Clear() { item = null; quantity = 0; }
    public bool Matches(ItemData other) => item == other;
}
