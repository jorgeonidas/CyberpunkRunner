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

    // Diccionarios para una búsqueda y gestión más eficientes
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
        // Si no hay efectos activos, no hacemos nada.
        if (_activeVfxTimers.Count == 0)
        {
            return;
        }

        // Usamos una lista para guardar los IDs de los VFX que han expirado.
        // Esto evita modificar el diccionario mientras lo estamos recorriendo.
        List<string> expiredVfxIds = new List<string>();

        // Iteramos sobre una copia de las claves para poder modificar el diccionario de forma segura.
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

        // Ahora procesamos los VFX que han expirado.
        foreach (string expiredId in expiredVfxIds)
        {
            if (_vfxInstances.TryGetValue(expiredId, out ParticleSystem vfxInstance))
            {
                vfxInstance.Stop();
            }
            _activeVfxTimers.Remove(expiredId);
        }
    }

    private void Player_OnPlayerDied()
    {
        _vehicleExhaust.StopExhaustEffect();
        StopAllLoopVFX();
    }

    /// <summary>
    /// Instancia y prepara todos los sistemas de partículas definidos al inicio.
    /// </summary>
    private void InitializeVFX()
    {
        if (_vfxTransform == null)
        {
            _vfxTransform = transform; // Usa el transform de este objeto como fallback.
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

    /// <summary>
    /// Activa un efecto de partículas en bucle por una duración determinada.
    /// </summary>
    /// <param name="vfxId">El identificador del VFX a reproducir.</param>
    /// <param name="duration">La duración en segundos que el efecto estará activo.</param>
    public void PlayLoopVFX(string vfxId, float duration)
    {
        if (!_vfxInstances.TryGetValue(vfxId, out ParticleSystem vfxInstance))
        {
            Debug.LogError($"VFX con id '{vfxId}' no encontrado.");
            return;
        }

        if (duration <= 0)
        {
            Debug.LogWarning($"La duración para el VFX '{vfxId}' es cero o negativa. El efecto no se reproducirá con temporizador.");
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

        // Añade o actualiza el temporizador para este VFX.
        if (_activeVfxTimers.ContainsKey(vfxId))
        {
            _activeVfxTimers[vfxId] += duration;
        }
        else
        {
            _activeVfxTimers.Add(vfxId, duration);
        } 
    }

    /// <summary>
    /// Detiene todos los efectos de partículas en bucle que estén activos.
    /// </summary>
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
