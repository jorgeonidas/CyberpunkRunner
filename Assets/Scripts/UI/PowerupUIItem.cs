using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUIItem : MonoBehaviour
{
    [SerializeField] private Image _powerUpIcon;
    [SerializeField] private Image _progressBar;
    private float duration;
    private float remainingTime;
    private string powerupId;
    PowerupUIManager _powerUpUiManager;
    public void Initialize(PowerupBase powerup, PowerupUIManager powerUpUiManager)
    {
        duration = powerup.Duration;
        remainingTime = duration;
        _powerUpUiManager = powerUpUiManager;
        _powerUpIcon.sprite = powerup.Icon;
        powerupId = powerup.Id;
        UpdateProgressBar();
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
}
