using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerupHandler : MonoBehaviour
{
    Player _player;
    [SerializeField] SerializedDictionary<string, PowerupBase> _activePowerups = new SerializedDictionary<string, PowerupBase>();
    [SerializeField] SerializedDictionary<string, float> _powerupTimers = new SerializedDictionary<string, float>();
    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    void Start()
    {
        PowerupPickup.OnAnyPowerupPicked += PowerupPickup_OnAnyPowerupPicked;
    }

    private void Update()
    {

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

        //revertir y remover powerups expirados
        foreach (var id in expiredPowerups)
        {
            if (_activePowerups.TryGetValue(id, out var effect))
            {
                effect.RevertEffect();
                _activePowerups.Remove(id);
            }
            _powerupTimers.Remove(id);
        }
    }

    private void OnDisable()
    {
        PowerupPickup.OnAnyPowerupPicked -= PowerupPickup_OnAnyPowerupPicked;
    }

    private void PowerupPickup_OnAnyPowerupPicked(PowerupBase pickedPowerUp)
    {
        Debug.Log($"Player picked {pickedPowerUp}");

        if (string.IsNullOrEmpty(pickedPowerUp.Id))
        {
            Debug.LogError($"Invalid powerup id for {pickedPowerUp} is null or empty");
            return;
        }
        
        float powerupDuration = pickedPowerUp.Duration;
        string powerUpId = pickedPowerUp.Id;
        _player.OnPowerUpActivated?.Invoke(powerUpId,powerupDuration);
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
}
