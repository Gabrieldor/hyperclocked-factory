using System;
using UnityEngine;

/// <summary>
/// Singleton. Owns all player-held items — 8 hotbar slots + 36 inventory slots.
///
/// HOTBAR (indices 0–7):  always visible; one slot is "selected" for placement.
/// INVENTORY (indices 0–35): 4 rows × 9 columns; opened from toolbar.
///
/// Add flow:  hotbar first (stack or empty), then inventory, then fail (full).
/// Move flow: MoveInventoryToHotbar() → first empty hotbar slot, or swap with selected.
/// Consume:   TryConsumeSelected() removes 1 from the currently selected hotbar slot.
///
/// Events fire on any data change — UI subscribes to these rather than polling.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    public const int HotbarSize    = 5;
    public const int InventoryRows = 4;
    public const int InventoryCols = 9;
    public const int InventorySize = InventoryRows * InventoryCols; // 36

    // ── Events ───────────────────────────────────────────────────────────────
    /// Fired whenever any slot content changes.
    public event Action OnInventoryChanged;
    /// Fired when the selected hotbar index changes (-1 = nothing selected).
    public event Action<int> OnHotbarSelectionChanged;

    // ── State ────────────────────────────────────────────────────────────────
    private readonly InventorySlot[] _hotbar    = MakeSlots(HotbarSize);
    private readonly InventorySlot[] _inventory = MakeSlots(InventorySize);

    public int SelectedHotbarIndex { get; private set; } = -1;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ── Selection ─────────────────────────────────────────────────────────────

    /// Selects the slot, or deselects it if it's already selected (toggle).
    public void SelectHotbarSlot(int index)
    {
        if (index < 0 || index >= HotbarSize) return;
        SelectedHotbarIndex = (SelectedHotbarIndex == index) ? -1 : index;
        OnHotbarSelectionChanged?.Invoke(SelectedHotbarIndex);
    }

    public void ClearSelection()
    {
        SelectedHotbarIndex = -1;
        OnHotbarSelectionChanged?.Invoke(-1);
    }

    /// The ItemData in the currently selected hotbar slot, or null if nothing is selected.
    public ItemData GetSelectedItem() =>
        SelectedHotbarIndex >= 0 ? _hotbar[SelectedHotbarIndex].item : null;

    // ── Slot accessors ────────────────────────────────────────────────────────
    public InventorySlot GetHotbarSlot(int i)    => _hotbar[i];
    public InventorySlot GetInventorySlot(int i) => _inventory[i];

    // ── Add ───────────────────────────────────────────────────────────────────

    /// Adds qty of item. Tries stacking first, then empty slots (hotbar before inventory).
    /// Returns false if completely full.
    public bool TryAddItem(ItemData item, int qty = 1)
    {
        if (TryStack(item, qty, _hotbar))    return true;
        if (TryStack(item, qty, _inventory)) return true;
        if (TryEmpty(item, qty, _hotbar))    return true;
        if (TryEmpty(item, qty, _inventory)) return true;
        return false;
    }

    private static bool TryStack(ItemData item, int qty, InventorySlot[] slots)
    {
        foreach (var s in slots)
            if (!s.IsEmpty && s.Matches(item) && s.quantity + qty <= item.stackSize)
            { s.quantity += qty; return true; }
        return false;
    }

    private static bool TryEmpty(ItemData item, int qty, InventorySlot[] slots)
    {
        foreach (var s in slots)
            if (s.IsEmpty) { s.Set(item, qty); return true; }
        return false;
    }

    // ── Consume ───────────────────────────────────────────────────────────────

    /// Removes 1 from the currently selected hotbar slot. Clears slot if it reaches 0.
    /// Returns false if nothing selected or slot empty.
    public bool TryConsumeSelected()
    {
        if (SelectedHotbarIndex < 0) return false;
        return TryConsumeFromHotbar(SelectedHotbarIndex);
    }

    public bool TryConsumeFromHotbar(int slotIndex, int qty = 1)
    {
        var s = _hotbar[slotIndex];
        if (s.IsEmpty || s.quantity < qty) return false;
        s.quantity -= qty;
        if (s.quantity <= 0) s.Clear();
        OnInventoryChanged?.Invoke();
        return true;
    }

    // ── Move ──────────────────────────────────────────────────────────────────

    /// Moves an inventory slot to the first empty hotbar slot.
    /// If the hotbar is full and a slot is selected, swaps with the selected slot.
    public void MoveInventoryToHotbar(int inventoryIndex)
    {
        var src = _inventory[inventoryIndex];
        if (src.IsEmpty) return;

        // First empty hotbar slot
        for (int i = 0; i < HotbarSize; i++)
        {
            if (_hotbar[i].IsEmpty)
            {
                _hotbar[i].Set(src.item, src.quantity);
                src.Clear();
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        // Hotbar full — swap with selected slot
        if (SelectedHotbarIndex >= 0)
        {
            var sel = _hotbar[SelectedHotbarIndex];
            (sel.item, src.item)         = (src.item, sel.item);
            (sel.quantity, src.quantity) = (src.quantity, sel.quantity);
            OnInventoryChanged?.Invoke();
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    private static InventorySlot[] MakeSlots(int n)
    {
        var arr = new InventorySlot[n];
        for (int i = 0; i < n; i++) arr[i] = new InventorySlot();
        return arr;
    }
}
