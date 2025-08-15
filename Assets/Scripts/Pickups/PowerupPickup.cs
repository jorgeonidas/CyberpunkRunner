using System;
using UnityEngine;

public class PowerupPickup : PickUp
{
    //public static Action<PowerupBase> OnAnyPowerupPicked;
    [SerializeField] PowerupActivationEvent _powerUpActivationEvent;
    [SerializeField] PowerupBase _powerUp;

    protected override void OnPickUp()
    {
        //OnAnyPowerupPicked?.Invoke(_powerUp);
        _powerUpActivationEvent.Raise(_powerUp);
    }
}
