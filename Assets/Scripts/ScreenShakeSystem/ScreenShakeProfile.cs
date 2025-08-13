using UnityEngine;

[CreateAssetMenu(fileName = "ScreenShakeProfile", menuName = "Screen Shake System/ScreenShakeProfile")]
public class ScreenShakeProfile : ScriptableObject
{
    [Header("Impulse Source Settings")]
    [SerializeField] float _impactTime;
    [SerializeField] float _impactForce;
    [SerializeField] Vector3 _defaultVelocity = new Vector3(0, -1, 0);
    [SerializeField] AnimationCurve _impulseCurve;

    [Header("Impulse Listener Settings")]
    [SerializeField] private float _listenerAmplitude = 1f;
    [SerializeField] private float _listenerFrequency = 1f;
    [SerializeField] private float _listenerDuration = 1f;

    public float ImpactTime { get => _impactTime; }
    public float ImpactForce { get => _impactForce; }
    public Vector3 DefaultVelocity { get => _defaultVelocity; }
    public AnimationCurve ImpulseCurve { get => _impulseCurve; }
    public float ListenerAmplitude { get => _listenerAmplitude; }
    public float ListenerFrequency { get => _listenerFrequency; }
    public float ListenerDuration { get => _listenerDuration; }
}
