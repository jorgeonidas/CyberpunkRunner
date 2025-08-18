using System;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private SpeedSettings _speedSettings;
    [SerializeField] private ParticleSystem _speedUpParticleSystem;
    [SerializeField] SpeedChangedEvent _speedChangedEvent;

    private void OnEnable()
    {
        _speedChangedEvent.OnEventRaised += HandleSpeedChanged;
    }

    private void OnDisable()
    {
        _speedChangedEvent.OnEventRaised -= HandleSpeedChanged;
    }

    private void HandleSpeedChanged(float speed)
    {
        //Debug.Log($"Speed changed to: {speed}");
        if (speed >= _speedSettings.InitialChunkSpeed)
        {
            PlaySpeedUpEffect();
        }
        else
        {
            StopSpeedUpEffect();
        }
    }

    public void PlaySpeedUpEffect()
    {
        if (_speedUpParticleSystem != null)
        {
            _speedUpParticleSystem.Play();
        }
        else
        {
            Debug.LogWarning("Speed Up Particle System is not assigned in the inspector.");
        }
    }

    public void StopSpeedUpEffect()
    {
        if (_speedUpParticleSystem != null)
        {
            _speedUpParticleSystem.Stop();
        }
        else
        {
            Debug.LogWarning("Speed Up Particle System is not assigned in the inspector.");
        }
    }
}
