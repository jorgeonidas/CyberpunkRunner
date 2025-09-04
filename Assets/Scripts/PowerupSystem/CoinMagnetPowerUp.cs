using UnityEngine;

[CreateAssetMenu(fileName = "CoinMagnetPowerUp", menuName = "Powerups/CoinMagnetPowerUp")]
public class CoinMagnetPowerUp : PowerupBase
{
    public override void RevertEffect()
    {
        GameManager.Instance.Player.CoinMagnet.MagnetActive = false;
    }

    protected override void ApplyEffect()
    {
        GameManager.Instance.Player.CoinMagnet.MagnetActive = true;
    }
}
