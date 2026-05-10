using UnityEngine;

/// <summary>
/// Handles camera zoom (pinch / scroll) and pan (drag) for the factory grid view.
///
/// ZOOM:
///   Touch: two-finger pinch spread/close → adjusts orthographicSize
///   Mouse: scroll wheel → adjusts orthographicSize
///   Clamped to [minZoom, maxZoom].
///
/// PAN:
///   Touch: single-finger drag (once InputReader confirms it's a drag, not a tap)
///   Mouse: right-click drag
///   Camera is clamped so the visible area never moves fully outside the grid.
///   When zoomed out far enough that the grid fits entirely on screen, the camera
///   is centered on the grid instead of free-panning.
///
/// COORDINATE HELPER:
///   ScreenToCell(screenPos) converts a screen position to a grid cell, or null if
///   the position is outside the grid. Used by placement and selection systems.
///
/// DEPENDENCIES:
///   InputReader — must exist in the scene (added by HF > Setup Input & Camera).
///   GridManager — used by ScreenToCell for bounds checking (optional; falls back
///                 to a raw floor if absent).
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────
    [Header("Zoom")]
    [Tooltip("Closest zoom level (orthographic size)")]
    [SerializeField] private float minZoom = 4f;
    [Tooltip("Furthest zoom level — shows full 16×16 grid with margin")]
    [SerializeField] private float maxZoom = 12f;
    [Tooltip("Zoom speed for pinch gesture (per screen-pixel of pinch change)")]
    [SerializeField] private float pinchZoomSpeed = 0.05f;
    // Scroll zoom sensitivity lives in InputReader — adjust it there.

    [Header("Pan Bounds")]
    [Tooltip("Grid width in cells — must match GridManager.width")]
    [SerializeField] private int gridWidth  = 16;
    [Tooltip("Grid height in cells — must match GridManager.height")]
    [SerializeField] private int gridHeight = 16;
    [Tooltip("How many world units (tiles) the camera can scroll past the grid edge on any side")]
    [SerializeField] private float maxOverpan = 4f;

    // ── Private ───────────────────────────────────────────────────────────────
    private Camera _cam;

    // ─────────────────────────────────────────────────────────────────────────

    private void Awake() => _cam = GetComponent<Camera>();

    // LateUpdate so all input state from InputReader.Update() is settled first.
    private void LateUpdate()
    {
        if (InputReader.Instance == null) return;

        ApplyZoom();
        ApplyPan();
        ClampPosition();
    }

    // ── Zoom ─────────────────────────────────────────────────────────────────

    private void ApplyZoom()
    {
        float delta = 0f;

        // Pinch: spreading fingers = negative delta (zooming in = smaller ortho size)
        if (InputReader.Instance.IsPinching)
            delta -= InputReader.Instance.PinchScreenDelta * pinchZoomSpeed;

        // Mouse scroll: pre-scaled by InputReader.scrollZoomSensitivity
        delta += InputReader.Instance.ScrollZoomDelta;

        if (Mathf.Approximately(delta, 0f)) return;
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize + delta, minZoom, maxZoom);
    }

    // ── Pan ───────────────────────────────────────────────────────────────────

    private void ApplyPan()
    {
        if (!InputReader.Instance.IsDragging) return;

        // Convert screen-pixel delta to world-unit delta.
        // orthographicSize covers half the screen height in world units.
        float worldPerPixel = (_cam.orthographicSize * 2f) / Screen.height;
        Vector2 screenDelta = InputReader.Instance.ScreenDragDelta;

        // Dragging right moves the scene right, so the camera moves left.
        transform.position += new Vector3(-screenDelta.x, -screenDelta.y, 0f) * worldPerPixel;
    }

    // ── Clamp ─────────────────────────────────────────────────────────────────

    private void ClampPosition()
    {
        float halfH = _cam.orthographicSize;
        float halfW = halfH * _cam.aspect;

        // maxOverpan = world units the viewport edge can travel past the grid edge.
        // Applied identically to all four sides → equal pan freedom in X and Y
        // regardless of aspect ratio.
        //
        // Derivation per axis (X shown):
        //   Left  viewport edge ≥ -maxOverpan  →  camX ≥ halfW - maxOverpan
        //   Right viewport edge ≤ gridWidth + maxOverpan  →  camX ≤ gridWidth - halfW + maxOverpan
        //
        // Safety: if the viewport is wider than grid + 2×overpan (only at extreme zoom-out
        // on wide screens), fall back to centring so xMin ≤ xMax is always true.

        float xMin = halfW  - maxOverpan;
        float xMax = gridWidth  - halfW  + maxOverpan;
        float yMin = halfH  - maxOverpan;
        float yMax = gridHeight - halfH + maxOverpan;

        float x = xMin <= xMax ? Mathf.Clamp(transform.position.x, xMin, xMax) : gridWidth  * 0.5f;
        float y = yMin <= yMax ? Mathf.Clamp(transform.position.y, yMin, yMax) : gridHeight * 0.5f;

        transform.position = new Vector3(x, y, transform.position.z);
    }

    // ── Public helpers ────────────────────────────────────────────────────────

    /// <summary>
    /// Converts a screen position to a grid cell.
    /// Returns null if the position falls outside the grid bounds.
    /// Used by placement and selection systems.
    /// </summary>
    public Vector2Int? ScreenToCell(Vector2 screenPos)
    {
        // For a 2D orthographic camera at z = -10, the distance to the world plane (z=0) is 10.
        float distToWorld = Mathf.Abs(transform.position.z);
        Vector3 world = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distToWorld));
        world.z = 0f;

        Vector2Int cell = GridManager.Instance != null
            ? GridManager.Instance.WorldToCell(world)
            : new Vector2Int(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

        if (GridManager.Instance != null && !GridManager.Instance.IsInBounds(cell))
            return null;

        return cell;
    }

    /// <summary>
    /// Returns the world position at the centre of a grid cell.
    /// Convenience wrapper for GridManager.CellToWorld.
    /// </summary>
    public Vector3 CellToWorld(Vector2Int cell) =>
        GridManager.Instance != null
            ? GridManager.Instance.CellToWorld(cell)
            : new Vector3(cell.x + 0.5f, cell.y + 0.5f, 0f);
}
