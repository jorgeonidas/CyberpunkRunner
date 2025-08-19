using System;
using UnityEngine;
using static SfxIdEnum;

public abstract class PowerupBase : ScriptableObject
{
    [SerializeField] string _id;
    [Header("Icon Image")]
    [SerializeField] private Sprite _icon;
    [Header("Sfx Loop Id")]
    [SerializeField] private LoopSfxId _loopSfxId = SfxIdEnum.LoopSfxId.None;

    [Header("If 0 it has inmediate effect")]
    [SerializeField] float _duration = 0;
    protected GameManager _gameManager = null;
    public string Id => _id;
    public float Duration => _duration;
    public Sprite Icon => _icon;
    public LoopSfxId LoopSfxId => _loopSfxId;
    public void StartEffect(GameManager gameManager)
    {
        _gameManager = gameManager;
        ApplyEffect();
    }

    protected abstract void ApplyEffect();
    public abstract void RevertEffect();
}
