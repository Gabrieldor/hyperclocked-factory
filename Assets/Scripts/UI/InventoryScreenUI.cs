using TMPro;
using UnityEngine;

/// <summary>
/// Full-screen inventory panel (4×9 = 36 slots) opened from the toolbar Inventory button.
/// Also owns the PickupConfirmPanel shown when the player long-presses a placed machine.
///
/// Wiring: all child references assigned by InventoryUISetupEditor.
/// Data:   subscribes to PlayerInventory.OnInventoryChanged in Start().
/// </summary>
public class InventoryScreenUI : MonoBehaviour
{
    public static InventoryScreenUI Instance { get; private set; }

    [Header("Inventory Panel")]
    [SerializeField] private GameObject      inventoryPanel;
    [SerializeField] private InventorySlotUI[] slots;          // 36 elements

    [Header("Pickup Confirm Panel")]
    [SerializeField] private GameObject pickupConfirmPanel;
    [SerializeField] private TMP_Text   pickupConfirmLabel;

    private Vector2Int _pendingPickupCell;

    // ── Lifecycle ─────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        inventoryPanel.SetActive(false);
        pickupConfirmPanel.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Setup(i, OnSlotTapped);

        var inv = PlayerInventory.Instance;
        if (inv != null) inv.OnInventoryChanged += RefreshInventory;

        RefreshInventory();
    }

    private void OnDestroy()
    {
        var inv = PlayerInventory.Instance;
        if (inv != null) inv.OnInventoryChanged -= RefreshInventory;
    }

    // ── Inventory panel ───────────────────────────────────────────────────────

    public void ToggleInventory() => inventoryPanel.SetActive(!inventoryPanel.activeSelf);

    // Called by each slot button
    private void OnSlotTapped(int index)
    {
        PlayerInventory.Instance?.MoveInventoryToHotbar(index);
    }

    private void RefreshInventory()
    {
        var inv = PlayerInventory.Instance;
        if (inv == null) return;
        for (int i = 0; i < slots.Length; i++)
            slots[i].Refresh(inv.GetInventorySlot(i), false);
    }

    // ── Pickup confirm panel ──────────────────────────────────────────────────

    /// Called by PlacementController when a long-press lands on a placed machine.
    public void ShowPickupConfirm(Vector2Int cell, string machineName)
    {
        _pendingPickupCell = cell;
        pickupConfirmLabel.text = $"Pick up {machineName}?";
        pickupConfirmPanel.SetActive(true);
    }

    /// Confirm button — removes machine from grid and returns it to inventory.
    public void OnPickupConfirmed()
    {
        var placed = GridManager.Instance?.GetAt(_pendingPickupCell);
        if (placed != null)
        {
            if (placed.sourceItem != null)
                PlayerInventory.Instance?.TryAddItem(placed.sourceItem);

            GridManager.Instance.TryRemove(_pendingPickupCell);
        }
        pickupConfirmPanel.SetActive(false);
    }

    /// Cancel button.
    public void OnPickupCancelled() => pickupConfirmPanel.SetActive(false);
}
