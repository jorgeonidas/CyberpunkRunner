using UnityEngine;

public class CoinPickUp : PickUp
{
    protected override void OnPickUp()
    {
        ScoreManager.OnCoinPickedEvent?.Invoke(10);//TODO: a setting for coins
    }
}
