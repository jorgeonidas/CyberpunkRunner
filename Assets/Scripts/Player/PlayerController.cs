using System;
using UnityEngine;
using UnityEngine.InputSystem;
    
public class PlayerController : MonoBehaviour
{
    public Action OnPausedPressed;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private SpeedSettings _speedSettings;

    private float[] _lanes;
    private Rigidbody _playerRigidbody;
    private Vector3 _targetPosition;

    private int _currentLane = 0;
    private float _defaultSideMoveSpeed = 5f;
    private float _currentSideMoveSpeed = 5f;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InitializePlayerCurrentLane();
        InitializeMoveSpeed();
        SetTargetLanePosition();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void InitializeMoveSpeed()
    {
        _defaultSideMoveSpeed = _speedSettings.PlayerSideMovingSpeed;
        _currentSideMoveSpeed = _defaultSideMoveSpeed;
    }

    private void InitializePlayerCurrentLane()
    {
        _lanes = _levelSettings.Lanes;
        _currentLane = _lanes.Length / 2;
    }

    // === Common lane-change helper used by both actions and swipe ===
    private void ChangeLane(int delta)
    {
        int newLane = Mathf.Clamp(_currentLane + delta, 0, _lanes.Length - 1);
        if (newLane == _currentLane) return; // already at edge
        _currentLane = newLane;
        SetTargetLanePosition();
    }

    // === Input Actions (keyboard/gamepad) ===
    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed) ChangeLane(-1);
    }

    public void MoveRigth(InputAction.CallbackContext context) // keeping your original name
    {
        if (context.performed) ChangeLane(+1);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPausedPressed?.Invoke();
        }
    }

    // === Public methods for swipe calls ===
    public void MoveLeft()
    {
        ChangeLane(-1);
    }

    public void MoveRight()
    {
        ChangeLane(+1);
    }

    public void AddMoveSpeed(float speed)
    {
        _currentSideMoveSpeed += speed;
    }

    private void HandleMovement()
    {
        Vector3 newPosition = Vector3.MoveTowards(
            _playerRigidbody.position,
            _targetPosition,
            _currentSideMoveSpeed * Time.fixedDeltaTime);

        _playerRigidbody.MovePosition(newPosition);
    }

    private void SetTargetLanePosition()
    {
        _targetPosition = _playerRigidbody.position;
        _targetPosition.x = _lanes[_currentLane];
    }
}
