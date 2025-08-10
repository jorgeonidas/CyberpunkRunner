using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public Action OnPlayerCollided;
    [SerializeField] float _hitCooldownTime = 1f;
    [SerializeField] float _adjustChangeMoveSpeedAmount = 1f;
    bool _hitCooldownActive;
    bool _isInvincible;
    float _hitCooldownTimer;

    private void Start()
    {
        _isInvincible = false;
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
        if (_isInvincible)
        {
            Destroy(other.gameObject);
            return;
        }

        if (_hitCooldownActive)
        {
            return;
        }
        Debug.Log(other.gameObject.name);
        ActivateHitCooldown();
        OnPlayerCollided?.Invoke();
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
        //some shield vfx
    }
}
