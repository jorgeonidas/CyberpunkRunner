using System;
using UnityEngine;

public enum GameState { Playing, Paused, GameOver }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] Player _player;
    [SerializeField] SpeedManager _speedManager;
    [SerializeField] LevelGenerator _leveGenerator;
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    [SerializeField] MovingObjectsSpawner _movingObjectsSpawner;
    [SerializeField] ScoreManager _scoreManager;
    GameState _currentGameState;
    CameraController _cameraController;
    public Player Player => _player;
    public float NormalizedChunkSpeed => _speedManager.NormalizedChunkSpeed;
    public GameState CurrentGameState => _currentGameState;
    public int CoinsCollected => _scoreManager == null ? 0 : _scoreManager.CoinsCollected;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Duplicated GameManager");
            Destroy(gameObject);
        }
        Instance = this;
        PlayerDataManager.Initialize();
        Debug.Log($"{PlayerDataManager.GetCoins()} coins collected | {PlayerDataManager.GetRecordDistance()} record distance");
    }

    private void Start()
    {
        _player.Initialize(this);
        _speedManager.OnSpeedDifficultyIncreased += SpeedManager_OnSpeedDifficultyIncreased;
        _player.OnPlayerDied += Player_OnPlayerDied;
        _player.OnPausePressed += PlayerController_OnPausedPressed;
        _leveGenerator.Initialize(_speedManager);
        _currentGameState = GameState.Playing;

        //play background music
        if(SfxManager.Instance != null)
        {
            SfxManager.Instance.PlayMusic(SfxIdEnum.SoundTrackId.GamePlay);
        }
    }

    void OnDisable()
    {
        _speedManager.OnSpeedDifficultyIncreased -= SpeedManager_OnSpeedDifficultyIncreased;
        _player.OnPausePressed -= PlayerController_OnPausedPressed;
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

    public int GetDistanceTravelled() => _leveGenerator.GetDistanceTravelled();

    public void SaveCollectedCoins()
    {
        _scoreManager.SaveCoinsCollected();
    }

    public void TooglePauseState()
    {
        if (_currentGameState == GameState.GameOver)
        {
            return;
        }
        _currentGameState = _currentGameState == GameState.Playing ? GameState.Paused : GameState.Playing;
        Debug.Log($"Game state changed to {_currentGameState}");
        _gameStateChangedEvent?.Raise(_currentGameState);
    }

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

    private void PlayerController_OnPausedPressed()
    {
        TooglePauseState();
    }
    
}
