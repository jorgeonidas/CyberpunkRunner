using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPlayerDied;
    public Action<string, float> OnPowerUpActivated;
    [SerializeField] GameObject _playerCharacterVisuals;
    PlayerController _playerController;
    PlayerCollisionHandler _playerCollisionHandler;
    private RagdollSpawner _ragdollSpawner;
    private bool _playerDead;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
        TryGetComponent(out _ragdollSpawner);
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
        _playerCharacterVisuals.SetActive(true);
    }

    public void SetInvincible(bool invincible)
    {
        _playerCollisionHandler.SetInvincible(invincible);
    }

    public void AddMoveSpeed(float speed)
    {
        _playerController.AddMoveSpeed(speed);
    }

    private void SetPlayerDead()
    {
        if (!_playerDead)
        {
            _playerDead = true;
            _playerController.enabled = false;
            _playerCollisionHandler.EnableCollider(false);
            _playerCharacterVisuals.SetActive(false);
            _ragdollSpawner?.SpawnRagdoll();
            PoolManager.Instance.Get("PlayerBike_Destroyed", transform.position, transform.rotation);
            OnPlayerDied?.Invoke();
            Debug.Log($"Player died");
        }
    }
}
