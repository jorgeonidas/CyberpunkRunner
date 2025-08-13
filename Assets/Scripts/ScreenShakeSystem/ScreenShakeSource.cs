using System;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenShakeSource : MonoBehaviour
{
    [SerializeField] ScreenShakeProfile _screenShakeProfile;
    [SerializeField] CinemachineImpulseSource _cinemachineImpulseSource;

    public void ShakeCamera()
    {
        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.ScreenShakeFromProfile(_screenShakeProfile, _cinemachineImpulseSource);
        }
    }
}
