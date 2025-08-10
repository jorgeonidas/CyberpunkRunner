using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoost", menuName = "Powerups/SpeedBoost")]
public class SpeedBoost : PowerupBase
{
    [Header("Speed Boosting")]
    [SerializeField] float _speedToAdd = 4f;
    protected override void ApplyEffect()
    {
        Debug.Log($"Applying speed boost");
        _gameManager.SpeedManager.AddSpeed(_speedToAdd);
    }

    public override void RevertEffect()
    {
        Debug.Log($"Speed boost Reverted");
        _gameManager.SpeedManager.AddSpeed(-_speedToAdd);
    }
}
