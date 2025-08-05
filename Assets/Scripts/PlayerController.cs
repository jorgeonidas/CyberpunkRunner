using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    Vector2 _movementInput;
    private Rigidbody _playerRigidbody;
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
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
        _playerRigidbody.MovePosition(newPosition);
    }
}
