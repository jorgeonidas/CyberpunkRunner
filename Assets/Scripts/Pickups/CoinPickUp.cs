using UnityEngine;

public class CoinPickUp : PickUp
{
    protected override void OnPickUp()
    {
        ScoreManager.OnAddScoreEvent?.Invoke(100);//TODO: a setting for coins
    }
}
