using UnityEngine;

[CreateAssetMenu(fileName = "RocketLauncherPowerup", menuName = "Powerups/RocketLauncherPowerup")]
public class RocketLauncherPowerup : PowerupBase
{
    [SerializeField] string _rocketPoolObjectId = "Rocket";
    [SerializeField] int _numberOfRockets = 3;
    [SerializeField] float _timeBetweenMissiles = 0.2f;
    public override void RevertEffect() { }

    protected override void ApplyEffect()
    {
        GameManager.Instance.Player.RocketLauncher.LaunchRockets(_rocketPoolObjectId, _timeBetweenMissiles, _numberOfRockets);
    }
}
