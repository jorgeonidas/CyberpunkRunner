using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Powerups/SpeedBoost")]
public class SpeedBoost : PowerupBase
{
    [Header("Speed Boosting")]
    [SerializeField] float _speedToAdd = 4f;
    protected override void ApplyEffect(Player player, GameManager gameManager)
    {
        Debug.Log($"Applying speed boost");
    }

    public override void RevertEffect()
    {
        Debug.Log($"Speed boost Reverted");
    }
}
