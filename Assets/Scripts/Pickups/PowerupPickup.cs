using System;
using UnityEngine;

public class PowerupPickup : PickUp
{
    [SerializeField] PowerupActivationEvent _powerUpActivationEvent;
    [SerializeField] PowerupBase _powerUp;

    protected override void OnPickUp()
    {
        _powerUpActivationEvent.Raise(_powerUp);
    }
}
