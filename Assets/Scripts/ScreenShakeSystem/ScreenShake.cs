using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }
    [SerializeField] CinemachineImpulseListener _cinemachineImpulseListener;
    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one ScreenShake in the scene {transform} {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        _cinemachineImpulseSource.GenerateImpulse(intensity);
    }

    public void ScreenShakeFromProfile(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        SetupScreenShakeSettings(profile, impulseSource);
        impulseSource.GenerateImpulseWithForce(profile.ImpactForce);
    }

    private void SetupScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        //Impulse source settings
        CinemachineImpulseDefinition cinemachineImpulseDefinition = impulseSource.ImpulseDefinition;
        cinemachineImpulseDefinition.ImpulseDuration = profile.ImpactTime;
        impulseSource.DefaultVelocity = profile.DefaultVelocity;
        impulseSource.ImpulseDefinition.CustomImpulseShape = profile.ImpulseCurve;

        //Impulse listener settings
        _cinemachineImpulseListener.ReactionSettings.AmplitudeGain = profile.ListenerAmplitude;
        _cinemachineImpulseListener.ReactionSettings.FrequencyGain = profile.ListenerFrequency;
        _cinemachineImpulseListener.ReactionSettings.Duration = profile.ListenerDuration;

    }
}
