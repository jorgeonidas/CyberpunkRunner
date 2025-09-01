using System;
using TMPro;
using UnityEngine;

public abstract class SettingsUIItemBase<TKey, TValue> : MonoBehaviour
{
    [SerializeField] protected TKey settingId;
    [SerializeField] protected TextMeshProUGUI _settingLabelText;
    private TValue currentValue;
    protected Action<TKey, TValue> OnValueChanged;

    public TKey SettingId => settingId;

    public virtual void Initialize(TValue value, Action<TKey, TValue> onValueChanged)
    {
        SetSettingLabel();
        currentValue = value;
        OnValueChanged = onValueChanged;
    }

    public virtual void UpdateValue(TValue newValue)
    {
        currentValue = newValue;
        OnValueChanged?.Invoke(settingId, currentValue);
        //Debug.Log($"Setting {settingId} updated to {currentValue}");
    }

    public abstract void SetSettingLabel();
}
