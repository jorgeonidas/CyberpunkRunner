using System;
using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private FloatSettingUIItem[] _floatSettingsUIItems;
    UserSettingsController _userSettingsController => UserSettingsController.Instance;
    private void OnEnable()
    {
        foreach (var setting in _floatSettingsUIItems)
        {
            Debug.Log($"{setting.SettingId} value {_userSettingsController.GetFloatSetting(setting.SettingId)}");
            setting.Initialize(_userSettingsController.GetFloatSetting(setting.SettingId), OnFloatSettingChanged);
        }
    }

    private void OnFloatSettingChanged(UserSettingsType.FloatSettingId id, float value)
    {
        _userSettingsController.SetFloatSetting(id, value);
    }

    void OnDisable()
    {
        _userSettingsController.SaveSettings();
    }
}
