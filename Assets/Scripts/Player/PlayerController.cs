using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] private float _moveSpeed = 5f;
    private float[] _lanes;
    private Rigidbody _playerRigidbody;
    private Vector3 _targetPosition;
    private int _currentLane = 0;
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _lanes = _levelSettings.Lanes;
        _currentLane = _lanes.Length / 2;
        SetTargetLanePosition();
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

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 newPosition = Vector3.MoveTowards(_playerRigidbody.position, _targetPosition, _moveSpeed * Time.fixedDeltaTime);
        _playerRigidbody.MovePosition(newPosition);
    }

    private void SetTargetLanePosition()
    {
        _targetPosition = _playerRigidbody.position;
        _targetPosition.x = _lanes[_currentLane];
    }
}
