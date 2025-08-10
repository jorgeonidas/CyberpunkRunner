using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilityPowerUp", menuName = "Powerups/InvincibilityPowerUp")]
public class InvincibilityPowerUp : PowerupBase
{
    protected override void ApplyEffect()
    {
        GameManager.Instance.Player.SetInvincible(true);
    }
    public override void RevertEffect()
    {
        GameManager.Instance.Player.SetInvincible(false);
    }
}
