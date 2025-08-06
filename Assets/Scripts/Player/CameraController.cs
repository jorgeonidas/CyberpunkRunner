using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _minFOV = 20f;
    [SerializeField] float _maxFOV = 80;
    [SerializeField] float _zoomDuration = 1f;
    [SerializeField] float _zoomSpeedModifier = 5;
    [SerializeField] ParticleSystem _speedUpParticleSystem;
    CinemachineCamera _cinemachineCamera;
    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }
    public void ChangeCaeramFOV(float speedAmount)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(speedAmount));
        //TODO: controlling by speed?
        if (speedAmount > 0)
        {
            _speedUpParticleSystem.Play();
        }
        else
        {
            _speedUpParticleSystem.Stop();
        }
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = _cinemachineCamera.Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedAmount + speedAmount * _zoomSpeedModifier, _minFOV, _maxFOV);

        float elapsedTime = 0;
        while (elapsedTime < _zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / _zoomDuration);
            yield return null;
        }
        _cinemachineCamera.Lens.FieldOfView = targetFOV;
    }
}
