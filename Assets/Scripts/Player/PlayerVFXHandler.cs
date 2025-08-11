using System;
using UnityEngine;

public class PlayerVFXHandler : MonoBehaviour
{
    [Serializable]
    public struct LoopVfxData
    {
        public string vfxId;
        public ParticleSystem _loopVfxPrevab;
    }
    [SerializeField] Transform _vfxTransform;
    [SerializeField] LoopVfxData[] _loopVfxData;
    [SerializeField] VehicleExhaust _vehicleExhaust;

    private Player _player;
    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _player.OnPlayerDied += Player_OnPlayerDied;
    }

    private void OnDisable()
    {
        _player.OnPlayerDied -= Player_OnPlayerDied;
    }

    private void Player_OnPlayerDied()
    {
        _vehicleExhaust.StopExhaustEffect();
    }
}
