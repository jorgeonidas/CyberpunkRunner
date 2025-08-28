using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UserSettingsController;
using static UserSettingsType;

public class FloatSettingUIItem : SettingsUIItemBase<FloatSettingId, float>
{
    [SerializeField] private Slider _slider;

    public override void Initialize(float value, Action<FloatSettingId, float> onValueChanged)
    {
        base.Initialize(value, onValueChanged);
        _slider.value = value;
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public override void SetSettingLabel() => _settingLabelText.text = settingId.ToStringDisplay();

    private void OnSliderValueChanged(float newValue)
    {
        UpdateValue(newValue);
        OnValueChanged?.Invoke(settingId, newValue);
    }
}
