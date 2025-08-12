using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerVFXHandler : MonoBehaviour
{
    [Serializable]
    public struct LoopVfxData
    {
        public string vfxId;
        public ParticleSystem loopVfxPrefab;
    }

    [SerializeField] Transform _vfxTransform;
    [SerializeField] VehicleExhaust _vehicleExhaust;
    [SerializeField] private LoopVfxData[] _loopVfxData;
    private readonly Dictionary<string, ParticleSystem> _vfxInstances = new Dictionary<string, ParticleSystem>();
    private readonly Dictionary<string, float> _activeVfxTimers = new Dictionary<string, float>();

    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
        InitializeVFX();
    }

    private void Start()
    {
        _player.OnPlayerDied += Player_OnPlayerDied;
        _player.OnPowerUpActivated += PlayLoopVFX;
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.OnPlayerDied -= Player_OnPlayerDied;
        }
        // Detiene todos los efectos activos cuando el componente se desactiva.
        StopAllLoopVFX();
        _player.OnPowerUpActivated -= PlayLoopVFX;  
    }


    private void Update()
    {
        if (_activeVfxTimers.Count == 0)
        {
            return;
        }

        List<string> expiredVfxIds = new List<string>();

        foreach (string vfxId in _activeVfxTimers.Keys.ToList())
        {
            float remainingTime = _activeVfxTimers[vfxId];
            remainingTime -= Time.deltaTime;
            _activeVfxTimers[vfxId] = remainingTime;
            if (remainingTime <= 0)
            {
                expiredVfxIds.Add(vfxId);
            }
        }

        foreach (string expiredId in expiredVfxIds)
        {
            if (_vfxInstances.TryGetValue(expiredId, out ParticleSystem vfxInstance))
            {
                vfxInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            _activeVfxTimers.Remove(expiredId);
        }
    }

    private void Player_OnPlayerDied()
    {
        _vehicleExhaust.StopExhaustEffect();
        StopAllLoopVFX();
    }

    private void InitializeVFX()
    {
        if (_vfxTransform == null)
        {
            _vfxTransform = transform;
            Debug.LogWarning("VFX Transform no está asignado. Usando el transform de este objeto por defecto.");
        }

        foreach (var vfxData in _loopVfxData)
        {
            if (vfxData.loopVfxPrefab != null && !string.IsNullOrEmpty(vfxData.vfxId))
            {
                ParticleSystem vfxInstance = Instantiate(vfxData.loopVfxPrefab, _vfxTransform);
                vfxInstance.gameObject.SetActive(false); // Empiezan inactivos.
                _vfxInstances.Add(vfxData.vfxId, vfxInstance);
            }
            else
            {
                Debug.LogWarning($"Entrada de LoopVfxData inválida. Falta el Prefab o el ID.");
            }
        }
    }
    
    public void PlayLoopVFX(string vfxId, float duration)
    {
        if (!_vfxInstances.TryGetValue(vfxId, out ParticleSystem vfxInstance))
        {
            Debug.LogError($"VFX with id '{vfxId}' not found");
            return;
        }

        if (duration <= 0)
        {
            Debug.LogWarning($"Vfx duration with id '{vfxId}' is zero or negative.");
            return;
        }

        if (!vfxInstance.gameObject.activeSelf)
        {
            vfxInstance.gameObject.SetActive(true);
        }

        if (!vfxInstance.isPlaying)
        {
            vfxInstance.Play();
        }

        if (_activeVfxTimers.ContainsKey(vfxId))
        {
            _activeVfxTimers[vfxId] += duration;
        }
        else
        {
            _activeVfxTimers.Add(vfxId, duration);
        }
    }

    private void StopAllLoopVFX()
    {
        foreach (var vfxInstance in _vfxInstances.Values)
        {
            if (vfxInstance.isPlaying)
            {
                vfxInstance.Stop();
            }
        }
        _activeVfxTimers.Clear();
    }
}
