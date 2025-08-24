using UnityEngine;

public static class StringConstants
{
    public const string Vfx_prefix = "VFX_";
    public const string PLAYER_TAG = "Player";
    public const string OBSTACLE_TAG = "Obstacle";
    public const string Coin = "Coin";
    public const string DestroyedSufix = "_Destroyed";
    public const string VehicleObstaclePrefix = "VehicleObstacle_";
    public class AnimatioTriggers
    {
        public static readonly string HIT = "Hit";
    }

    public class PowerupIds
    {
        public const string None = "None";
        public const string SpeedBoost = "SpeedBoost";
        public const string Invincible = "Invincible";
    }

    public class IngamePanels
    {
        public const string Pause = "Pause";
        public const string GameOver = "GameOver";
        public const string IngameHud = "IngameHud";
    }

    public class MainMenuPanels
    {
        public const string MainMenu = "MainMenu";
        public const string SettingsMenu = "SettingsMenu";
        // public const string ShopMenu = "ShopMenu";
        // public const string CharacterSelectionMenu = "CharacterSelectionMenu";
    }
}
