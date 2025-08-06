using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _horizontalLimit = 4f;
    [SerializeField] private float zMaxLimit = 2f;
    [SerializeField] private float zMinLimit = -1f;
    Vector2 _movementInput;
    private Rigidbody _playerRigidbody;
    VehicleLeaning _vehicleLaning;
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _vehicleLaning = GetComponent<VehicleLeaning>();
    }
    public void Move(InputAction.CallbackContext conext)
    {
        _movementInput = conext.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 currentPosition = _playerRigidbody.position;
        Vector3 moveDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
        Vector3 newPosition = currentPosition + (moveDirection * Time.fixedDeltaTime * _moveSpeed);
        newPosition.x = Mathf.Clamp(newPosition.x, -_horizontalLimit, _horizontalLimit);
        newPosition.z = Mathf.Clamp(newPosition.z, zMinLimit, zMaxLimit);
        _playerRigidbody.MovePosition(newPosition);
        if (_vehicleLaning)
        {
            _vehicleLaning.LeanHorizontal(_movementInput.x);
        }
    }
}
