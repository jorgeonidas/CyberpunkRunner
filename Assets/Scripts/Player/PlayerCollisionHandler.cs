using System;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
   // [SerializeField] Animator _animator;
    [SerializeField] float _hitCooldownTime = 1f;
    [SerializeField] float _adjustChangeMoveSpeedAmount = 1f;
    bool _hitCooldownActive;
    float _hitCooldownTimer;

    private void Start()
    {
        //just in case there is an obstacle when starts the match
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
        //_animator.SetTrigger(StringConstants.AnimatioTriggers.HIT);
        ActivateHitCooldown();
        LevelGenerator.OnChangeSpeedAmount?.Invoke(-_adjustChangeMoveSpeedAmount);
    }

    private void ActivateHitCooldown()
    {
        _hitCooldownTimer = _hitCooldownTime;
        _hitCooldownActive = true;
    }
}
