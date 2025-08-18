using System;
using UnityEngine;

public enum GameState { Playing, Paused, GameOver }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    //public Action OnGameOver;
    [SerializeField] Player _player;
    [SerializeField] SpeedManager _speedManager;
    [SerializeField] LevelGenerator _leveGenerator;
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    [SerializeField] MovingObjectsSpawner _movingObjectsSpawner;
    GameState _currentGameState;
    CameraController _cameraController;
    public Player Player => _player;
    public float NormalizedChunkSpeed => _speedManager.NormalizedChunkSpeed;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Duplicated GameManager");
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start()
    {
        _player.Initialize(this);
        _player.OnPlayerDied += Player_OnPlayerDied;
        _leveGenerator.Initialize(_speedManager);
        _currentGameState = GameState.Playing;
    }

    private void OnEnable()
    {
        _speedManager.OnSpeedDifficultyIncreased += SpeedManager_OnSpeedDifficultyIncreased;
    }

    void OnDisable()
    {
        _speedManager.OnSpeedDifficultyIncreased -= SpeedManager_OnSpeedDifficultyIncreased;
        _player.OnPlayerDied -= Player_OnPlayerDied;
    }
    public void SetCameraController(CameraController cameraController)
    {
        _cameraController = cameraController;
    }
    public void AddSpeed(float speed)
    {
        _speedManager.AddSpeedBonus(speed);
        _player.AddMoveSpeed(speed);
        _cameraController.ChangeCaeramFOV(speed);
    }

    public float GetDistanceTravelled() => _leveGenerator.GetDistanceTravelled();

    private void Player_OnPlayerDied()
    {
        //stop everything
        if (_currentGameState != GameState.GameOver)
        {
            _currentGameState = GameState.GameOver;
            _speedManager.Stop(true);
            _gameStateChangedEvent?.Raise(_currentGameState);
        }
    }

    private void SpeedManager_OnSpeedDifficultyIncreased()
    {
        _movingObjectsSpawner.TryDecreaseSpawnrate();
    }
}
