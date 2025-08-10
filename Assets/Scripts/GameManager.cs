using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] SpeedManager _speedManager;
    [SerializeField] LevelGenerator _leveGenerator;

    private void Awake()
    {

    }

    private void Start()
    {
        _player.OnPlayerDied += Playuer_OnPlayerDied;
        _leveGenerator.Initialize(_speedManager);
    }

    private void OnEnable()
    {

    }

    void OnDisable()
    {
        _player.OnPlayerDied -= Playuer_OnPlayerDied;
    }

    private void Playuer_OnPlayerDied()
    {
        //stop everything
    }
}
