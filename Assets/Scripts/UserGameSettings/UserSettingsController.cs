using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using static UserSettingsType;

[CreateAssetMenu(fileName = "UserSettingsController", menuName = "User Settings/UserGameSettingsController")]
public class UserSettingsController : SingletonScriptableObject<UserSettingsController>
{
    [Serializable]
    public struct FloatSetting
    {
        public float defaultValue;
        public float minValue;
        public float maxValue;
    }
    public Action<FloatSettingId, float> OnFloatSettingChanged;
    [SerializeField] SerializedDictionary<FloatSettingId, FloatSetting> _floatSettings;
    private UserGameSettings _userGameSettings;
    private UserDataServiceSO _userData;
    override protected void OnInitialize()
    {
        _userData = UserDataServiceSO.Instance;
        _userGameSettings = _userData.GetSettings();
    }

    public void SetFloatSetting(FloatSettingId settingType, float value)
    {
        if (_floatSettings.ContainsKey(settingType))
        {
            var setting = _floatSettings[settingType];
            float clampedValue = Mathf.Clamp(value, setting.minValue, setting.maxValue);
            switch (settingType)
            {
                case FloatSettingId.MusicVolume:
                    _userGameSettings.musicVolume = clampedValue;
                   // Debug.Log($"Music Volume set to: {clampedValue}");
                    break;
                case FloatSettingId.SFXVolume:
                    _userGameSettings.sfxVolume = clampedValue;
                    //Debug.Log($"SFX Volume set to: {clampedValue}");
                    break;
                default:
                    Debug.LogWarning($"Unhandled setting type: {settingType}");
                    break;
            }
            OnFloatSettingChanged?.Invoke(settingType, clampedValue);
        }
        else
        {
            Debug.LogWarning($"Setting type {settingType} not found in settings dictionary.");
        }
    }

    public float GetFloatSetting(FloatSettingId settingType)
    {
        if (_floatSettings.ContainsKey(settingType))
        {
            switch (settingType)
            {
                case FloatSettingId.MusicVolume:
                    return _userGameSettings != null ? _userGameSettings.musicVolume : 1;
                case FloatSettingId.SFXVolume:
                    return _userGameSettings != null ? _userGameSettings.sfxVolume : 1;
                default:
                    Debug.LogWarning($"Unhandled setting type: {settingType}");
                    return 1f;
            }
        }
        else
        {
            Debug.LogWarning($"Setting type {settingType} not found in settings dictionary.");
            return 1f;
        }
    }

    public void SetToDefaultSettings()
    {
        _userGameSettings.musicVolume = _floatSettings[UserSettingsType.FloatSettingId.MusicVolume].defaultValue;
        _userGameSettings.sfxVolume = _floatSettings[UserSettingsType.FloatSettingId.SFXVolume].defaultValue;
        Debug.Log("User settings set to default values.");
        SaveSettings();
    }

    public void SaveSettings()
    {
        _userData.UpdateGameSettings(_userGameSettings);
    }   
}
