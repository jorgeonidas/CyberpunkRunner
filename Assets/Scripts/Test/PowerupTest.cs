using UnityEngine;
using UnityEngine.InputSystem;

public class PowerupTest : MonoBehaviour
{
    private TestInputs _controls;
    [SerializeField] DestructibleObstacle[] _obstacles;
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

    private void OnActivatePowerup(InputAction.CallbackContext ctx)
    {
        // Aquí llamas a tu lógica de prueba
        // Example: PowerupManager.Instance.TryActivateDebugPowerup();
        if (ctx.performed == true)
        {
            Debug.Log("[Debug] Powerup triggered by Space.");
            Rocket newRocket = Instantiate(_rocketPrefab, transform.position, Quaternion.identity);
            newRocket.Launch(_obstacles[0].transform);
        }
    }
}
