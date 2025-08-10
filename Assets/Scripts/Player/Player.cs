using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPlayerDied;
    PlayerController _playerController;
    PlayerCollisionHandler _playerCollisionHandler;
    private bool _playerDead;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
    }

    private void OnEnable()
    {
        _playerCollisionHandler.OnPlayerCollided += PlayerCollisionHandle_OnPlayerCollided;
    }

    private void OnDisable()
    {
        _playerCollisionHandler.OnPlayerCollided += PlayerCollisionHandle_OnPlayerCollided;
    }

    private void PlayerCollisionHandle_OnPlayerCollided()
    {
        SetPlayerDead();
    }

    public void Initialize()
    {

    }

    private void SetPlayerDead()
    {
        if (!_playerDead)
        {
            _playerDead = true;
            OnPlayerDied?.Invoke();
            Debug.Log($"Player died");
        }
    }
}
