using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch      = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// <summary>
/// Singleton pointer state machine — touch + mouse parity.
///
/// STATE MACHINE:
///   None → (pointer down) → MaybeTap
///   MaybeTap → (moved > dragThreshold) → Dragging
///   MaybeTap → (held > longPressTime, no move) → fires OnLongPress → None
///   MaybeTap → (released, no move) → fires OnTap → None
///   Dragging → (second finger) → Pinching
///   Pinching → (one finger lifted) → Dragging → None
///
/// Per-frame outputs (reset to zero each Update):
///   ScreenDragDelta  — screen-space pixels moved this frame (single finger or right-click drag)
///   PinchScreenDelta — change in distance between two fingers this frame (positive = spreading)
///   IsDragging       — true when in Dragging state
///   IsPinching       — true when in Pinching state
///
/// Touch  → left-click equivalent for taps, right-click drag equivalent for pan via single finger.
/// Mouse  → left-click = tap, right-click drag = pan, scroll = zoom (handled in CameraController).
/// </summary>
[DefaultExecutionOrder(-100)]   // runs before CameraController and placement code
public class InputReader : MonoBehaviour
{
    public static InputReader Instance { get; private set; }

    // ── Events ───────────────────────────────────────────────────────────────
    /// Fired when a tap is confirmed (pointer released without significant movement).
    /// Argument: screen position of the tap.
    public event Action<Vector2> OnTap;

    /// Fired when a finger/button is held without moving past the drag threshold.
    /// Argument: screen position where the hold started.
    public event Action<Vector2> OnLongPress;

    // ── Per-frame outputs (read by CameraController) ─────────────────────────
    public Vector2 ScreenDragDelta  { get; private set; }
    public float   PinchScreenDelta { get; private set; }
    public float   ScrollZoomDelta  { get; private set; }  // pre-scaled; feed directly into CameraController
    public bool    IsDragging       { get; private set; }
    public bool    IsPinching       { get; private set; }

    // ── Tunable thresholds ───────────────────────────────────────────────────
    [Header("Thresholds")]
    [Tooltip("Pixel movement before a press becomes a drag")]
    [SerializeField] private float dragThreshold  = 10f;
    [Tooltip("Seconds held without moving before OnLongPress fires")]
    [SerializeField] private float longPressTime  = 0.5f;

    [Header("Zoom Sensitivity")]
    [Tooltip("How much each scroll-wheel tick changes the camera's orthographic size. Higher = faster zoom.")]
    [SerializeField] private float scrollZoomSensitivity = 3f;

    // ── Internal state ───────────────────────────────────────────────────────
    private enum State { None, MaybeTap, Dragging, Pinching }
    private State   _state;
    private Vector2 _pressScreenPos;
    private float   _pressTime;
    private float   _prevPinchDist;
    private bool    _longPressConsumed;

    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()  => EnhancedTouchSupport.Enable();
    private void OnDisable() => EnhancedTouchSupport.Disable();

    private void Update()
    {
        // Reset per-frame outputs
        ScreenDragDelta  = Vector2.zero;
        PinchScreenDelta = 0f;
        ScrollZoomDelta  = 0f;
        IsDragging  = false;
        IsPinching  = false;

        var touches = Touch.activeTouches;

        if      (touches.Count >= 2) HandlePinch(touches);
        else if (touches.Count == 1) HandleSingleTouch(touches[0]);
        else                         HandleMouse();
    }

    // ── Touch handlers ───────────────────────────────────────────────────────

    private void HandleSingleTouch(Touch t)
    {
        // If we were pinching and one finger lifted, reset cleanly
        if (_state == State.Pinching)
        {
            _state = State.None;
            return;
        }

        switch (t.phase)
        {
            case TouchPhase.Began:
                _state             = State.MaybeTap;
                _pressScreenPos    = t.screenPosition;
                _pressTime         = Time.unscaledTime;
                _longPressConsumed = false;
                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                if (_state == State.MaybeTap)
                {
                    if (Vector2.Distance(t.screenPosition, _pressScreenPos) > dragThreshold)
                    {
                        _state = State.Dragging;
                    }
                    else if (!_longPressConsumed && Time.unscaledTime - _pressTime >= longPressTime)
                    {
                        _longPressConsumed = true;
                        OnLongPress?.Invoke(_pressScreenPos);
                        _state = State.None;
                    }
                }
                if (_state == State.Dragging)
                {
                    ScreenDragDelta = t.delta;
                    IsDragging = true;
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (_state == State.MaybeTap && !_longPressConsumed)
                    OnTap?.Invoke(t.screenPosition);
                _state = State.None;
                break;
        }
    }

    private void HandlePinch(System.Collections.Generic.IReadOnlyList<Touch> touches)
    {
        float dist = Vector2.Distance(touches[0].screenPosition, touches[1].screenPosition);

        if (_state != State.Pinching)
        {
            _prevPinchDist = dist;   // first frame of pinch — no delta yet
            _state = State.Pinching;
        }

        PinchScreenDelta = dist - _prevPinchDist;
        _prevPinchDist   = dist;
        IsPinching = true;
    }

    // ── Mouse handler ────────────────────────────────────────────────────────

    private void HandleMouse()
    {
        var mouse = UnityEngine.InputSystem.Mouse.current;
        if (mouse == null) return;

        // ── Left click → tap ──────────────────────────────────────────────
        if (mouse.leftButton.wasPressedThisFrame)
        {
            _pressScreenPos = mouse.position.ReadValue();
        }
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            float moved = Vector2.Distance(mouse.position.ReadValue(), _pressScreenPos);
            if (moved <= dragThreshold)
                OnTap?.Invoke(mouse.position.ReadValue());
        }

        // ── Right-click drag → pan ────────────────────────────────────────
        if (mouse.rightButton.wasPressedThisFrame)
        {
            _state          = State.MaybeTap;
            _pressScreenPos = mouse.position.ReadValue();
        }

        if (mouse.rightButton.isPressed)
        {
            Vector2 pos = mouse.position.ReadValue();
            if (_state == State.MaybeTap &&
                Vector2.Distance(pos, _pressScreenPos) > dragThreshold)
                _state = State.Dragging;

            if (_state == State.Dragging)
            {
                ScreenDragDelta = mouse.delta.ReadValue();
                IsDragging = true;
            }
        }

        if (mouse.rightButton.wasReleasedThisFrame)
            _state = State.None;

        // ── Scroll wheel → zoom ───────────────────────────────────────────
        float scroll = mouse.scroll.ReadValue().y;
        if (scroll != 0f)
            ScrollZoomDelta = -scroll * scrollZoomSensitivity * Time.unscaledDeltaTime;
    }
}
