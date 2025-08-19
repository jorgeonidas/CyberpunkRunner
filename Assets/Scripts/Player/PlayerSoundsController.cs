using System;
using UnityEngine;

public class PlayerSoundsController : MonoBehaviour
{
    [SerializeField] private float _engineTotalPitch = 3f;
    LoopSfxEmmiter _engineSoundEmmiter;
    Player _player;
    void Awake()
    {
        TryGetComponent(out _engineSoundEmmiter);
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if(_player.IsPlayerDead)
        {
            return;
        }
        _engineSoundEmmiter?.SetLoopPitch(_player.CurrentNormalizedSpeed * _engineTotalPitch);
    }

    public void PlayEngineLoopSfx()
    {
        _engineSoundEmmiter?.PlayLoopSfx();
    }

    public void StopEngineLoopSfx()
    {
        _engineSoundEmmiter?.StopLoopSfx();
    }
}
