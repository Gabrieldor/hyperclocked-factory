using UnityEngine;

/// <summary>
/// Always-visible hotbar strip at the bottom of the screen.
/// Manages 8 InventorySlotUI views. Tracks which slot is selected.
///
/// Wiring: slots[] is assigned by InventoryUISetupEditor.
/// Data:   subscribes to PlayerInventory events in Start() — guaranteed after all Awake()s.
/// </summary>
public class HotbarUI : MonoBehaviour
{
    public static HotbarUI Instance { get; private set; }

    [SerializeField] private InventorySlotUI[] slots; // 8 elements, set by editor script

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Wire each slot button
        for (int i = 0; i < slots.Length; i++)
            slots[i].Setup(i, OnSlotTapped);

        // Subscribe to inventory changes
        var inv = PlayerInventory.Instance;
        if (inv != null)
        {
            inv.OnInventoryChanged       += Refresh;
            inv.OnHotbarSelectionChanged += _ => Refresh();
        }

        Refresh();
    }

    private void OnDestroy()
    {
        var inv = PlayerInventory.Instance;
        if (inv != null)
        {
            inv.OnInventoryChanged       -= Refresh;
            inv.OnHotbarSelectionChanged -= _ => Refresh();
        }
    }

    // Called by each slot button via InventorySlotUI.Setup
    public void OnSlotTapped(int index) => PlayerInventory.Instance?.SelectHotbarSlot(index);

    private void Refresh()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null) return;
        int sel = inv.SelectedHotbarIndex;
        for (int i = 0; i < slots.Length; i++)
            slots[i].Refresh(inv.GetHotbarSlot(i), i == sel);
    }
}
