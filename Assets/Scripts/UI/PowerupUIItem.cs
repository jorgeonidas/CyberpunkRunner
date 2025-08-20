using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIItem : MonoBehaviour, IGameStateChangedListener
{
    [SerializeField] private Image _powerUpIcon;
    [SerializeField] private Image _progressBar;
    [SerializeField] private GameStateChangedEvent _gameStateChangedEvent;
    private float duration;
    private float remainingTime;
    private string powerupId;
    PowerupUIManager _powerUpUiManager;
    public GameState CurrentGameState { get; set; }

    public void Initialize(PowerupBase powerup, PowerupUIManager powerUpUiManager)
    {
        duration = powerup.Duration;
        remainingTime = duration;
        _powerUpUiManager = powerUpUiManager;
        _powerUpIcon.sprite = powerup.Icon;
        powerupId = powerup.Id;
        UpdateProgressBar();
    }

    private void OnEnable()
    {
        _gameStateChangedEvent.OnEventRaised += OnGameStateChanged;
    }

    void OnDisable()
    {
        _gameStateChangedEvent.OnEventRaised -= OnGameStateChanged;
    }

    public void AddTime(float additionalTime)
    {
        remainingTime += additionalTime;
        if (remainingTime > duration)
        {
            duration = remainingTime;
        }
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Paused)
        {
            return;
        }

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateProgressBar();

            if (remainingTime <= 0)
            {
                _powerUpUiManager.RemovePowerup(powerupId);
            }
        }
    }

    private void UpdateProgressBar()
    {
        if (_progressBar != null)
        {
            _progressBar.fillAmount = remainingTime / duration;
        }
    }

    public void OnGameStateChanged(GameState newGameState)
    {
        CurrentGameState = newGameState;
    }
}
