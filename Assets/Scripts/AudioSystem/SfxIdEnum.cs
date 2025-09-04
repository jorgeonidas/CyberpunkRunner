using UnityEngine;

public class SfxIdEnum
{
    public enum SfxId
    {
        None = 0,
        CoinPickup = 1,
        Impact = 2,
        SpeedPowerup = 3,
        InvincibilityPowerup = 4,
        Whoosh = 5,
        RocketLaunch = 6,
        //UI
    }

    public enum UISfxId
    {
        Click = 0,
        Back = 1,
        Slide = 2,
        Confirm = 3,
        Cancel = 4,
        Purchased = 5,
        EquipPaint = 6,
    }

    public enum LoopSfxId
    {
        None = 0,
        BackgroundMusic = 1,
        AmbientSound = 2,
        PlayerBikeEngineLoop = 3,
        ForceFieldLoop = 4,
        CarEngineLoop = 5,
    }

    public enum SoundTrackId
    {
        None = 0,
        MainMenu = 1,
        GamePlay = 2,
        GameOver = 3,
        Victory = 4,
    }
}
