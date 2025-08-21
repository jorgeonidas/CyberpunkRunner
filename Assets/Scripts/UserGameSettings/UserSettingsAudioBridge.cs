using UnityEngine;

public class UserSettingsAudioBridge : MonoBehaviour
{
    SoundMixerManager _soundMixerManager;
    void Awake()
    {
        _soundMixerManager = GetComponent<SoundMixerManager>();
    }

    private void Start()
    { 
        // Initialize audio settings based on user preferences
        _soundMixerManager.SetSfxVolume(UserSettingsController.Instance.GetFloatSetting(UserSettingsType.FloatSettingId.SFXVolume));
        _soundMixerManager.SetMusicVolime(UserSettingsController.Instance.GetFloatSetting(UserSettingsType.FloatSettingId.MusicVolume));
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
            case UserSettingsType.FloatSettingId.MusicVolume:
                _soundMixerManager.SetSfxVolume(UserSettingsController.Instance.GetFloatSetting(UserSettingsType.FloatSettingId.SFXVolume));
                break;
            case UserSettingsType.FloatSettingId.SFXVolume:
                _soundMixerManager.SetSfxVolume(UserSettingsController.Instance.GetFloatSetting(UserSettingsType.FloatSettingId.SFXVolume));
                break;
            default:
                Debug.LogWarning($"Unhandled setting type: {id}");
                break;
        }
    }
}