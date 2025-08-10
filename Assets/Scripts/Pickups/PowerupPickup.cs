using System;
using UnityEngine;

public class PowerupPickup : PickUp
{
    public static Action<PowerupBase> OnAnyPowerupPicked;
    [SerializeField] PowerupBase _powerUp;

    protected override void OnPickUp()
    {
        OnAnyPowerupPicked?.Invoke(_powerUp);
    }
}
