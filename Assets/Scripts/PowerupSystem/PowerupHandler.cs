using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerupHandler : MonoBehaviour, IGameStateChangedListener
{
    Player _player;
    [SerializeField] SerializedDictionary<string, PowerupBase> _activePowerups = new SerializedDictionary<string, PowerupBase>();
    [SerializeField] SerializedDictionary<string, float> _powerupTimers = new SerializedDictionary<string, float>();
    [SerializeField] PowerupActivationEvent _powerupActivationEvent;
    [Header("Game state changed event")]
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    public GameState CurrentGameState { get; set; }

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    void Start()
    {
        _powerupActivationEvent.OnEventRaised += PowerupPickup_OnAnyPowerupPicked;
        _player.OnPlayerDied += Player_OnPlayerDied;
        _gameStateChangedEvent.OnEventRaised += OnGameStateChanged;

    }

    private void OnDisable()
    {
        _powerupActivationEvent.OnEventRaised -= PowerupPickup_OnAnyPowerupPicked;
        _player.OnPlayerDied -= Player_OnPlayerDied;
        _gameStateChangedEvent.OnEventRaised -= OnGameStateChanged;
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Paused)
        {
            return;
        }

        if (_powerupTimers.Count <= 0)
        {
            return;
        }

        List<string> powerupIds = _powerupTimers.Keys.ToList();
        var expiredPowerups = new List<string>();

        for (int i = 0; i < powerupIds.Count; i++)
        {
            string powerUpId = powerupIds[i];
            float powerUpTimer = _powerupTimers[powerUpId];
            float remaining = powerUpTimer - Time.deltaTime;
            if (remaining <= 0)
            {
                //powerup expired
                expiredPowerups.Add(powerUpId);
            }
            else
            {
                _powerupTimers[powerUpId] = remaining;
            }
        }

        //revert expired powerups
        foreach (var id in expiredPowerups)
        {
            RemoveExpiredPowerup(id);
        }
    }

    private void PowerupPickup_OnAnyPowerupPicked(PowerupBase pickedPowerUp)
    {
       // Debug.Log($"Player picked {pickedPowerUp}");
        if (_player.IsPlayerDead)
        {
            return;
        }

        if (string.IsNullOrEmpty(pickedPowerUp.Id))
        {
            Debug.LogError($"Invalid powerup id for {pickedPowerUp} is null or empty");
            return;
        }

        //Debug.Log($"powerUpIntId {pickedPowerUp.Id.GetHashCode()} for powerup {pickedPowerUp.Id}");

        float powerupDuration = pickedPowerUp.Duration;
        string powerUpId = pickedPowerUp.Id;
        _player.OnPowerUpActivated?.Invoke(powerUpId, powerupDuration);
        SfxManager.Instance.PlayLoopSfx(pickedPowerUp.LoopSfxId, transform, pickedPowerUp.Id.GetHashCode());
        if (_powerupTimers.ContainsKey(powerUpId))
        {
            float remaining = _powerupTimers[powerUpId];
            remaining += powerupDuration;
            _powerupTimers[powerUpId] = remaining;
        }
        else
        {
            pickedPowerUp.StartEffect(GameManager.Instance);
            _powerupTimers.Add(powerUpId, powerupDuration);
            _activePowerups.Add(powerUpId, pickedPowerUp);
        }
    }

    private void Player_OnPlayerDied()
    {
        ClearAllPowerUps();
    }

    private void RemoveExpiredPowerup(string id)
    {
        if (_activePowerups.TryGetValue(id, out var effect))
        {
            RevertPowerup(id, effect);
            _activePowerups.Remove(id);
        }
        _powerupTimers.Remove(id);
    }

    private void ClearAllPowerUps()
    {
        _powerupTimers.Clear();
        foreach (var kvp in _activePowerups)
        {
            RevertPowerup(kvp.Key, kvp.Value);
        }
        _activePowerups.Clear();
    }

    private void RevertPowerup(string id, PowerupBase effect)
    {
        effect.RevertEffect();
        SfxManager.Instance.StopLoopSfx(id.GetHashCode());
    }

    public void OnGameStateChanged(GameState newGameState)
    {
        CurrentGameState = newGameState;
    }
}
