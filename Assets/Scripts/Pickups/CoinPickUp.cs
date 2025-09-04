using UnityEngine;

public class CoinPickUp : PickUp
{
    [SerializeField] CurrencySetting _coinSetting;
    [SerializeField] MovingObject _movingObjectScript;

    public override void OnGetFromPool()
    {
        Magnetize(false);
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

    public void Magnetize(bool magnetize)
    {
        _movingObjectScript.enabled = !magnetize;
    }
}
