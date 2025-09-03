using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerCollisionHandler : MonoBehaviour
{
    // === Events ===
    /// <summary>Raised when the player takes a hit (and is not invincible nor under cooldown).</summary>
    public event Action OnPlayerCollided;

    // === Inspector ===
    [Header("Collision")]
    [Tooltip("Optional: Player collider. If null, it will be resolved from children on Awake.")]
    [SerializeField] private Collider _playerCollider;

    [Header("Feedback")]
    [Tooltip("Optional screen shake source.")]
    [SerializeField] private ScreenShakeSource _screenShakeSource;

    [Header("Rules")]
    [Tooltip("Cooldown (seconds) to ignore further hits after a valid collision.")]
    [SerializeField] private float _hitCooldownSeconds = 1f;

    [Header("Testing")]
    [Tooltip("When enabled, the player is always invincible regardless of runtime state.")]
    [SerializeField] private bool _forceInvincibleForTesting = false;

    // === State ===
    private float _lastHitTime = float.NegativeInfinity;  
    private bool _runtimeInvincible;                       

    // === Properties ===
    private bool IsOnCooldown => Time.time < _lastHitTime + _hitCooldownSeconds;
    private bool IsInvincible => _forceInvincibleForTesting || _runtimeInvincible;

    private void Awake()
    {
        if (_screenShakeSource == null)
        {
            TryGetComponent(out _screenShakeSource);
        }

        if (_playerCollider == null)
        {
            _playerCollider = GetComponentInChildren<Collider>();
        }

        if (_playerCollider == null)
        {
            Debug.LogWarning("[PlayerCollisionHandler] No collider found. Call EnableCollider(false) if intentional.", this);
        }
    }

    private void Start()
    {
        // Start with cooldown active so immediate spawn contacts don't trigger a hit
        ArmCooldown();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag(StringConstants.OBSTACLE_TAG))
        {
            return;
        }

        // Handle obstacle-specific logic
        TryHandleObstacleSide(other);

        if (IsInvincible || IsOnCooldown)
        {
            return;
        }

        ArmCooldown();
        OnPlayerCollided?.Invoke();
    }

    private void TryHandleObstacleSide(Collision other)
    {
        if (other.transform.TryGetComponent<IDestroy>(out var destroyable))
        {
            _screenShakeSource?.ShakeCamera();
            destroyable.DestroyMe();
        }
    }

    private void ArmCooldown()
    {
        _lastHitTime = Time.time;
    }

    public void SetInvincible(bool value)
    {
        _runtimeInvincible = value;

#if UNITY_EDITOR
        Debug.Log($"[PlayerCollisionHandler] Runtime invincible: {_runtimeInvincible}");
#endif
    }

    public void EnableCollider(bool enable)
    {
        if (_playerCollider == null)
        {
            Debug.LogWarning("[PlayerCollisionHandler] No collider to enable/disable.", this);
            return;
        }

        _playerCollider.enabled = enable;
    }
}
