using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class VehicleLeaning : MonoBehaviour
{
    [SerializeField] Transform _visual;           // modelo de la moto
    [SerializeField] float _maxLean = 45f;        // grados
    [SerializeField] float _smoothTime = 0.08f;   // suavizado
    [SerializeField] float _speedToMaxLean = 8f;  // m/s para alcanzar maxLean

    Rigidbody _playerRigidBody;
    float currentLean, leanVel;
    Quaternion initialLocalRot;

    void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        initialLocalRot = _visual ? _visual.localRotation : Quaternion.identity;
    }

    void FixedUpdate()
    {
        float lateralSpeed = _playerRigidBody.linearVelocity.x;
        float normalizedMinusPlusLeaning = Mathf.Clamp(lateralSpeed / _speedToMaxLean, -1f, 1f);
        float target = normalizedMinusPlusLeaning * _maxLean;

        currentLean = Mathf.SmoothDampAngle(currentLean, target, ref leanVel, _smoothTime);

        if (_visual)
        {
            _visual.localRotation = initialLocalRot * Quaternion.Euler(0f, 0f, -currentLean);
        }
    }

}
