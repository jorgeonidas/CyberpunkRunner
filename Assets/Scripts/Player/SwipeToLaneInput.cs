// SwipeToLaneInput.cs
// Detects horizontal swipes and forwards them to PlayerController.
// Clean code: keeps input concerns separate from movement logic.

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[DefaultExecutionOrder(-50)] // ensure it initializes early
public class SwipeToLaneInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _player;

    [Header("Swipe Settings")]
    [Tooltip("Minimum horizontal distance (in pixels) to consider a swipe.")]
    [SerializeField] private float _minSwipeDistancePx = 80f;

    [Tooltip("Maximum duration (seconds) for a swipe gesture.")]
    [SerializeField] private float _maxSwipeTime = 0.5f;

    [Tooltip("How much stronger the horizontal must be than vertical. 1.2 = 20% more horizontal.")]
    [SerializeField] private float _horizontalDominance = 1.2f;

    [Tooltip("Optional dead zone in pixels to ignore micro-drags.")]
    [SerializeField] private float _deadZonePx = 10f;

    private Vector2 _startPos;
    private double _startTime;
    private bool _tracking;

    private void Reset()
    {
        if (_player == null) _player = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;

        // Optional: also handle mouse (editor/desktop)
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        InputSystem.onDeviceChange -= OnDeviceChange;

        EnhancedTouchSupport.Disable();
    }

    private void OnFingerDown(Finger finger)
    {
        // Only track first active finger for simplicity
        if (_tracking) return;

        _tracking = true;
        _startPos = finger.screenPosition;
        _startTime = Time.timeAsDouble;
    }

    private void OnFingerUp(Finger finger)
    {
        if (!_tracking) return;
        _tracking = false;

        var endPos = finger.screenPosition;
        var duration = (float)(Time.timeAsDouble - _startTime);

        if (duration > _maxSwipeTime) return;

        var delta = endPos - _startPos;
        if (delta.magnitude < Mathf.Max(_deadZonePx, _minSwipeDistancePx)) return;

        // Horizontal dominance check
        if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y) * _horizontalDominance) return;

        if (delta.x > 0)
            _player?.MoveRight();
        else
            _player?.MoveLeft();
    }

    // -------- Optional: simple mouse fallback for Editor/Standalone --------
    private bool _mouseTracking;
    private Vector2 _mouseStart;
    private double _mouseStartTime;

    private void Update()
    {
        // Handle mouse only when Touchscreen is not present or you want both
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _mouseTracking = true;
            _mouseStart = Mouse.current.position.ReadValue();
            _mouseStartTime = Time.timeAsDouble;
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && _mouseTracking)
        {
            _mouseTracking = false;

            var end = Mouse.current.position.ReadValue();
            var duration = (float)(Time.timeAsDouble - _mouseStartTime);
            if (duration > _maxSwipeTime) return;

            var delta = end - _mouseStart;
            if (delta.magnitude < Mathf.Max(_deadZonePx, _minSwipeDistancePx)) return;
            if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y) * _horizontalDominance) return;

            if (delta.x > 0)
                _player?.MoveRight();
            else
                _player?.MoveLeft();
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Not required; placeholder if you want to react to device availability.
    }
}
