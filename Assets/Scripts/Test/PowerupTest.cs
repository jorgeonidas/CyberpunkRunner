using UnityEngine;
using UnityEngine.InputSystem;

public class PowerupTest : MonoBehaviour
{
    private TestInputs _controls;
    [SerializeField] Rocket _rocketPrefab;

    private void Awake()
    {
        _controls = new TestInputs();
    }

    private void OnEnable()
    {
        _controls.DebugInputs.SpaceBarInput.performed += OnActivatePowerup;
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.DebugInputs.SpaceBarInput.performed -= OnActivatePowerup;
        _controls.Disable();
    }

    private void OnActivatePowerup(InputAction.CallbackContext context)
    {
        if (context.performed == true)
        {

            var obstacles =  Object.FindObjectsByType<DestructibleObstacle>(FindObjectsSortMode.None);
            Debug.Log("[Debug] Powerup triggered by Space.");
            Rocket newRocket = Instantiate(_rocketPrefab, transform.position, Quaternion.identity);
            newRocket.Launch(obstacles[0].transform);
        }
    }
}
