using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupBase", menuName = "Scriptable Objects/PowerupBase")]
public abstract class PowerupBase : ScriptableObject
{
    [SerializeField] string _id;
    [Header("If 0 it has inmediate effect")]
    [SerializeField] float _duration = 0;
    protected Player _player = null;
    protected GameManager _gameManager = null;
    public string Id => _id;

    public float Duration => _duration;
    public void StartEffect(Player player, GameManager gameManager)
    {
        _player = player;
        _gameManager = gameManager;
        ApplyEffect();
    }

    protected abstract void ApplyEffect();

    public abstract void RevertEffect();

}
