using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action OnPlayerDied;
    public Action OnPausePressed;
    public Action<string, float> OnPowerUpActivated;
    [SerializeField] GameObject _playerCharacterVisuals;
    [SerializeField] GameStateChangedEvent _gameStateChangedEventListener;
    private GameManager _gameManager;
    PlayerController _playerController;
    PlayerCollisionHandler _playerCollisionHandler;
    PlayerSoundsController _playerSoundsController;
    private RocketLauncher _rocketLauncher;
    private CoinMagnet _coinMagnet;
    private RagdollSpawner _ragdollSpawner;
    private bool _playerDead;
    public bool IsPlayerDead => _playerDead;
    public float CurrentNormalizedSpeed => _gameManager.NormalizedChunkSpeed;
    public RocketLauncher RocketLauncher => _rocketLauncher;
    public CoinMagnet CoinMagnet => _coinMagnet;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
        TryGetComponent(out _ragdollSpawner);
        TryGetComponent(out _playerSoundsController);
        TryGetComponent(out _rocketLauncher);
    }

    private void OnEnable()
    {
        _playerCollisionHandler.OnPlayerCollided += PlayerCollisionHandle_OnPlayerCollided;
        _playerController.OnPausedPressed += PlayerController_OnPausedPressed;
        _gameStateChangedEventListener.OnEventRaised += OnGameStateChanged;

    }

    private void OnDisable()
    {
        _playerCollisionHandler.OnPlayerCollided += PlayerCollisionHandle_OnPlayerCollided;
        _playerController.OnPausedPressed += PlayerController_OnPausedPressed;
        _gameStateChangedEventListener.OnEventRaised -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.GameOver:
            case GameState.Paused:
                _playerController.enabled = false;
                break;
            case GameState.Playing:
                _playerController.enabled = true;
                break;
            default:
                break;
        }
    }

    private void PlayerCollisionHandle_OnPlayerCollided()
    {
        SetPlayerDead();
    }

    public void Initialize(GameManager gameManager)
    {
        if (!_playerCharacterVisuals.activeSelf)
        {
            _playerCharacterVisuals.SetActive(true);
        }
        _playerSoundsController.PlayEngineLoopSfx();
        _gameManager = gameManager;
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
            _playerSoundsController.StopEngineLoopSfx();
            PoolManager.Instance.Get("PlayerBike_Destroyed", transform.position, transform.rotation);
            OnPlayerDied?.Invoke();
            Debug.Log($"Player died");
        }
    }
    
    
    private void PlayerController_OnPausedPressed()
    {
        OnPausePressed?.Invoke();
    }
}
