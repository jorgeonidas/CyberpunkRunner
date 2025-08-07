using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    [SerializeField] float _hitCooldownTime = 1f;
    [SerializeField] float _adjustChangeMoveSpeedAmount = 1f;
    bool _hitCooldownActive;
    float _hitCooldownTimer;

    private void Start()
    {
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
        if (_hitCooldownActive)
        {
            return;
        }
        Debug.Log(other.gameObject.name);
        ActivateHitCooldown();
        LevelGenerator.OnChangeSpeedAmount?.Invoke(-_adjustChangeMoveSpeedAmount);
    }

    private void ActivateHitCooldown()
    {
        _hitCooldownTimer = _hitCooldownTime;
        _hitCooldownActive = true;
    }
}
