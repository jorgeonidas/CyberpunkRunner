using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public Action OnPlayerCollided;
    [SerializeField] float _hitCooldownTime = 1f;
    
    [Header("Test Invincible")]
    [SerializeField] bool _testInvincible = false;
    ScreenShakeSource _screenShakeSource;
    Collider _playerCollider;
    bool _hitCooldownActive;
    bool _isInvincible;
    float _hitCooldownTimer;

    private void Awake()
    {
        TryGetComponent(out _screenShakeSource);
        _playerCollider = GetComponent<Collider>(); 
    }

    private void Start()
    {
        _isInvincible = false;
        if (_testInvincible)
        {
            _isInvincible = _testInvincible;//carefull here
        }
        ActivateHitCooldown();
    }

    private void Update()
    {
        if (_hitCooldownActive)
        {
            _hitCooldownTimer -= Time.deltaTime;
            if (_hitCooldownTimer < 0)
            {
                _hitCooldownActive = false;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag(StringConstants.OBSTACLE_TAG))
        {
            return;
        }

        if (_isInvincible)
        {
            HandleInvincibleCollision(other);
        }
        else
        {
            HandleVulnerableCollision(other);
        }
    }

    private void HandleInvincibleCollision(Collision other)
    {
        if (other.transform.TryGetComponent<IDestroy>(out IDestroy obstacle))
        {
            _screenShakeSource?.ShakeCamera();
            obstacle.DestroyMe();
        }
    }

    private void HandleVulnerableCollision(Collision other)
    {
        if (_hitCooldownActive)
        {
            return;
        }

        ActivateHitCooldown();
        OnPlayerCollided?.Invoke();
        _screenShakeSource?.ShakeCamera();
    }

    private void ActivateHitCooldown()
    {
        _hitCooldownTimer = _hitCooldownTime;
        _hitCooldownActive = true;
    }

    public void SetInvincible(bool invincible)
    {
        _isInvincible = invincible;
        Debug.Log($"_isInvincible {_isInvincible}");
    }

    public void EnableCollider(bool enable)
    {
        _playerCollider.enabled = enable;
    }
}
