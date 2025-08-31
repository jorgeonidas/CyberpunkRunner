using UnityEngine;

public class CoinPickUp : PickUp
{
    [SerializeField] CurrencySetting _coinSetting;
    protected override void OnPickUp()
    {
        ScoreManager.OnCoinPickedEvent?.Invoke(_coinSetting.CoinValue);
    }
}
