using UnityEngine;

public class CoinPickUp : PickUp
{
    [SerializeField] CurrencySetting _coinSetting;

    public override void OnGetFromPool()
    {
        CoinsManager.Instance?.RegisterCoin(this);
        base.OnGetFromPool();
    }

    public override void OnReleaseToPool()
    {
        CoinsManager.Instance?.UnregisterCoin(this);
        base.OnReleaseToPool();
    }

    protected override void OnPickUp()
    {
        ScoreManager.OnCoinPickedEvent?.Invoke(_coinSetting.CoinValue);

    }
}
