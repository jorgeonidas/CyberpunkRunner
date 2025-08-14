using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float _minFOV = 60f;
    [SerializeField] float _maxFOV = 70;
    [SerializeField] float _zoomDuration = 1f;
    //[SerializeField] ParticleSystem _speedUpParticleSystem;
    CinemachineCamera _cinemachineCamera;
    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void Start()
    {
        _cinemachineCamera.Lens.FieldOfView = _minFOV;
        GameManager.Instance?.SetCameraController(this);
    }

    public void ChangeCaeramFOV(float speedAmount)
    {
        float targetFov = speedAmount > 0 ? _maxFOV : _minFOV;
        StopAllCoroutines();
        StartCoroutine(ChangeFOVRoutine(targetFov));
        //TODO: controlling by speed?
        // if (speedAmount > 0)
        // {
        //     _speedUpParticleSystem.Play();
        // }
        // else
        // {
        //     _speedUpParticleSystem.Stop();
        // }
    }

    IEnumerator ChangeFOVRoutine(float targetFOV)
    {
        float startFOV = _cinemachineCamera.Lens.FieldOfView;
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
