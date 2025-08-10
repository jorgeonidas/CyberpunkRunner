using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerupBase", menuName = "Scriptable Objects/PowerupBase")]
public abstract class PowerupBase : ScriptableObject
{
    [SerializeField] string _id;
    [Header("If 0 it has inmediate effect")]
    [SerializeField] float _duration = 0;

    public string Id => _id;

    public float StartEffect(Player player, GameManager gameManager)
    {
        return _duration;
        ApplyEffect(player, gameManager);
    }

    protected abstract void ApplyEffect(Player player, GameManager gameManager);

    public abstract void RevertEffect();

}
