using UnityEngine;

public class CoinPickUp : PickUp
{
    protected override void OnPickUp()
    {
        ScoreManager.OnScoreChanged?.Invoke(100);
    }
}
