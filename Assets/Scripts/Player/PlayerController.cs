using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Action OnPausedPressed;
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] SpeedSettings _speedSettings;
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

    public void MoveLeft(InputAction.CallbackContext conext)
    {
        if (conext.performed)
        {
            _currentLane = Mathf.Max(_currentLane - 1, 0);
            SetTargetLanePosition();
        }
    }

    public void MoveRigth(InputAction.CallbackContext conext)
    {
        if (conext.performed)
        {
            _currentLane = Mathf.Min(_currentLane + 1, _lanes.Length - 1);
            SetTargetLanePosition();
        }
    }

    public void TogglePause(InputAction.CallbackContext conext)
    {
        if (conext.performed)
        {
            OnPausedPressed?.Invoke();
        }
    }

    public void AddMoveSpeed(float speed)
    {
        _currentSideMoveSpeed += speed;
    }

    private void HandleMovement()
    {
        Vector3 newPosition = Vector3.MoveTowards(_playerRigidbody.position, _targetPosition, _currentSideMoveSpeed * Time.fixedDeltaTime);
        _playerRigidbody.MovePosition(newPosition);
    }

    private void SetTargetLanePosition()
    {
        _targetPosition = _playerRigidbody.position;
        _targetPosition.x = _lanes[_currentLane];
    }
}
