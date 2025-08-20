using static UserSettingsType;

public class UserSettingsType
{
    public enum FloatSettingId
    {
        MusicVolume,
        SFXVolume
    }
}

public static class UserSettingsTypeExtensions
{
    public static string ToStringDisplay(this FloatSettingId id)
    {
        switch (id)
        {
            case FloatSettingId.MusicVolume:
                return "Music Volume";
            case FloatSettingId.SFXVolume:
                return "SFX Volume";
            default:
                return id.ToString();
        }
    }
}
