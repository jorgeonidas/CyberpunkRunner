using UnityEngine;

public class UserSettingsAudioBridge : MonoBehaviour
{
    private void Start()
    {
        SfxManager.Instance.SetSfxVolume(UserSettingsController.Instance.GetFloatSetting(UserSettingsType.FloatSettingId.SFXVolume));
    }

    private void OnEnable()
    {
        UserSettingsController.Instance.OnFloatSettingChanged += OnFloatSettingChanged;
    }

    private void OnDisable()
    {
        UserSettingsController.Instance.OnFloatSettingChanged -= OnFloatSettingChanged;
    }

    private void OnFloatSettingChanged(UserSettingsType.FloatSettingId id, float value)
    {
        switch (id)
        {
            // case UserSettingsType.FloatSettingId.MusicVolume:
            //     AudioManager.Instance.SetMusicVolume(value);
            //     break;
            case UserSettingsType.FloatSettingId.SFXVolume:
                SfxManager.Instance.SetSfxVolume(value);
                break;
            default:
                Debug.LogWarning($"Unhandled setting type: {id}");
                break;
        }
    }
}