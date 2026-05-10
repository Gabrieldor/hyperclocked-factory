using UnityEngine;

/// <summary>
/// Bridges InputReader events to the grid and inventory systems.
///
/// TAP on grid cell:
///   If a hotbar item is selected and its ItemData.placeableMachine is set,
///   and the cell is empty → consumes 1 from the hotbar slot and calls GridManager.TryPlace.
///
/// LONG PRESS on grid cell:
///   If a machine exists at that cell → asks InventoryScreenUI to show the pickup confirm panel.
///
/// DEPENDENCIES: InputReader, CameraController (on Main Camera), GridManager, PlayerInventory.
/// Subscribes in Start() to guarantee all singletons are initialised first.
/// </summary>
[DefaultExecutionOrder(-50)]
public class PlacementController : MonoBehaviour
{
    private CameraController _camera;

    private void Start()
    {
        _camera = Camera.main?.GetComponent<CameraController>();

        if (InputReader.Instance != null)
        {
            InputReader.Instance.OnTap       += HandleTap;
            InputReader.Instance.OnLongPress += HandleLongPress;
        }
    }

    private void OnDestroy()
    {
        if (InputReader.Instance != null)
        {
            InputReader.Instance.OnTap       -= HandleTap;
            InputReader.Instance.OnLongPress -= HandleLongPress;
        }
    }

    // ── Tap → place ───────────────────────────────────────────────────────────

private void HandleTap(Vector2 screenPos)
    {
        if (NodeSelectionPanel.Instance != null && NodeSelectionPanel.Instance.IsOpen) return;
        if (_camera == null || GridManager.Instance == null) return;

        var cell = _camera.ScreenToCell(screenPos);
        if (cell == null) return;

        var inv = PlayerInventory.Instance;
        if (inv == null) return;

        int hotbarIndex = inv.SelectedHotbarIndex;

        if (hotbarIndex < 0)
        {
            NodeManager.Instance?.TryOpenNodePanel(cell.Value);
            return;
        }

        var slot = inv.GetHotbarSlot(hotbarIndex);
        if (slot.IsEmpty || slot.item == null) return;

        var machine = slot.item.placeableMachine;
        if (machine == null) return;

        if (!GridManager.Instance.IsCellEmpty(cell.Value)) return;

        if (!inv.TryConsumeFromHotbar(hotbarIndex)) return;

        GridManager.Instance.TryPlace(machine, cell.Value);

        if (inv.GetHotbarSlot(hotbarIndex).IsEmpty)
            inv.ClearSelection();
    }

    // ── Long press → pickup ───────────────────────────────────────────────────

    private void HandleLongPress(Vector2 screenPos)
    {
        if (_camera == null || GridManager.Instance == null) return;

        var cell = _camera.ScreenToCell(screenPos);
        if (cell == null) return;

        var placed = GridManager.Instance.GetAt(cell.Value);
        if (placed == null) return;                        // no machine here

        InventoryScreenUI.Instance?.ShowPickupConfirm(cell.Value, placed.data.machineName);
    }
}
