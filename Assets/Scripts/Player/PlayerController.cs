using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _horizontalLimit = 4f;
    [SerializeField] private float zMaxLimit = 2f;
    [SerializeField] private float zMinLimit = -1f;
    private Rigidbody _playerRigidbody;
    VehicleLeaning _vehicleaning;
    private Vector3 _targetPosition;
    //testing discrete movement
    //TODO: what if we want to put more lanes?
    //TODO: move to a level settings
    float _moveGap = 3f;
    private int _currentLane = 0; // -1 = left, 0 = center, 1 = rigth
    private readonly int _minLane = -1;
    private readonly int _maxLane = 1;
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _vehicleaning = GetComponent<VehicleLeaning>();
    }

    public void MoveLeft(InputAction.CallbackContext conext)
    {
        if (conext.performed)
        {
            Debug.Log($"MoveLeft");
            _currentLane = Mathf.Max(_currentLane - 1, _minLane);
            SetTargetLanePosition();
        }
    }

    public void MoveRigth(InputAction.CallbackContext conext)
    {
        if (conext.performed)
        {
            Debug.Log($"MoveRigth");
            _currentLane = Mathf.Min(_currentLane + 1, _maxLane);
            SetTargetLanePosition();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 newPosition = Vector3.MoveTowards(_playerRigidbody.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);
        _playerRigidbody.MovePosition(newPosition);
        if (_vehicleaning)
        {
            _vehicleaning.LeanHorizontal(_playerRigidbody.linearVelocity.normalized.x);
        }
    }

    private void SetTargetLanePosition()
    {
        _targetPosition = _playerRigidbody.position;
        _targetPosition.x = _currentLane * _moveGap;
    }
}
