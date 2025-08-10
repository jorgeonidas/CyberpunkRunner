using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action OnGameOver;
    [SerializeField] Player _player;
    [SerializeField] SpeedManager _speedManager;
    [SerializeField] LevelGenerator _leveGenerator;
    public SpeedManager SpeedManager => _speedManager;
    private bool _gameOver = false;
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
        //will generate the player
        _player.OnPlayerDied += Player_OnPlayerDied;
        _leveGenerator.Initialize(_speedManager);
        _gameOver = false;
    }

    private void OnEnable()
    {

    }

    void OnDisable()
    {
        _player.OnPlayerDied -= Player_OnPlayerDied;
    }

    private void Player_OnPlayerDied()
    {
        //stop everything
        if (!_gameOver)
        {
            _gameOver = true;
            _speedManager.Stop();
            OnGameOver?.Invoke();
        }
    }
}
