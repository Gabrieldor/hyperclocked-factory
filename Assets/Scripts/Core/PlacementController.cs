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
            // No item selected: open machine panel, port panel, or node panel
            if (GridManager.Instance.GetAt(cell.Value) is { } placed)
                MachineInfoPanel.Instance?.Open(placed);
            else if (GridManager.Instance.IsPipeAt(cell.Value))
                PipePortPanel.Instance?.Open(cell.Value);
            else
                NodeManager.Instance?.TryOpenNodePanel(cell.Value);
            return;
        }

        var slot = inv.GetHotbarSlot(hotbarIndex);
        if (slot.IsEmpty || slot.item == null) return;

        // Pipe placement
        if (slot.item.placeablePipe != null)
        {
            var pipeData = slot.item.placeablePipe;
            if (!GridManager.Instance.IsPipeAt(cell.Value) &&
                GridManager.Instance.IsCellEmpty(cell.Value))
            {
                if (inv.TryConsumeFromHotbar(hotbarIndex))
                {
                    GridManager.Instance.TryPlacePipe(pipeData, cell.Value);
                    var pipe = GridManager.Instance.GetPipeAt(cell.Value);
                    if (pipe != null) pipe.sourceItem = slot.item;
                    if (inv.GetHotbarSlot(hotbarIndex).IsEmpty) inv.ClearSelection();
                }
            }
            return;
        }

        // Machine placement
        var machine = slot.item.placeableMachine;
        if (machine == null) return;

        if (!GridManager.Instance.IsCellEmpty(cell.Value)) return;

        // Extractor must be placed on a resource node
        if (machine.isExtractor && !NodeManager.Instance.IsNodeCell(cell.Value)) return;

        if (!inv.TryConsumeFromHotbar(hotbarIndex)) return;

        GridManager.Instance.TryPlace(machine, cell.Value);
        var newMachine = GridManager.Instance.GetAt(cell.Value);
        if (newMachine != null) newMachine.sourceItem = slot.item;
        if (inv.GetHotbarSlot(hotbarIndex).IsEmpty) inv.ClearSelection();
    }

    // ── Long press → pickup ───────────────────────────────────────────────────

    private void HandleLongPress(Vector2 screenPos)
    {
        if (_camera == null || GridManager.Instance == null) return;

        var cell = _camera.ScreenToCell(screenPos);
        if (cell == null) return;

        // Long-press on pipe → remove and return to inventory
        if (GridManager.Instance.IsPipeAt(cell.Value))
        {
            var pipe = GridManager.Instance.GetPipeAt(cell.Value);
            if (pipe == null) return;
            var itemToReturn = pipe.sourceItem;
            GridManager.Instance.TryRemovePipe(cell.Value);
            if (itemToReturn != null)
                PlayerInventory.Instance?.TryAddItem(itemToReturn, 1);
            return;
        }

        var placed = GridManager.Instance.GetAt(cell.Value);
        if (placed == null) return;

        InventoryScreenUI.Instance?.ShowPickupConfirm(cell.Value, placed.data.machineName);
    }
}
