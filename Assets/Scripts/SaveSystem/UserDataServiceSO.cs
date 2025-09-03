using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[CreateAssetMenu(fileName = "UserDataServiceSO", menuName = "Game/Services/UserDataService")]
public class UserDataServiceSO : SingletonScriptableObject<UserDataServiceSO>
{
    private UserData _data;
    private JsonSerializerSettings _jsonSettings;

    public event Action OnCurrencyChanged;
    public event Action<ProductCategory, string> OnInventoryChanged;
    public event Action<ProductCategory, string> OnEquippedChanged;
    public Action<ProductCategory, string> OnPreviewItem;
    public Action<ProductCategory> OnRevertItem;

    [Header("Storage")]
    [SerializeField] private string fileName = "userdata.json";

    // ----- Init / Load / Save -----
    public void Initialize()
    {
        //Json Config
        _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        _jsonSettings.Converters.Add(new StringEnumConverter());

        _data = SaveToJson.Load<UserData>(fileName, _jsonSettings) ?? new UserData();
        FailSafeData();
        Debug.Log($"[UserDataService] Initialized {fileName} _data? {_data != null} gs {_data.userGameSettings != null}");
    }

    private void FailSafeData()
    {
        if (_data.userGameSettings == null)
        {
            _data.userGameSettings = new UserGameSettings();
        }
    }

    public void Save()
    {
        SaveToJson.Save(fileName, _data, _jsonSettings);
    }
    // ----- Read API -----
    public int GetCoins() => _data.CoinsCollected;
    public int GetRecordDistance() => _data.RecordDistance;
    public UserGameSettings GetSettings() => _data.userGameSettings;

    public void UpdateGameSettings(UserGameSettings settings)
    {
        _data.userGameSettings = settings;
        Save();
    }

    public IReadOnlyList<string> GetOwned(ProductCategory category)
        => _data.GetOwnedProductsByCategory(category);

    public string GetEquipped(ProductCategory category)
        => _data.GetEquippedProductInCategory(category);

    public bool Owns(ProductCategory category, string productId)
        => _data.CheckIfProductIsOwned(category, productId);

    public bool IsEquipped(ProductCategory category, string productId)
        => _data.CheckIfProductIsEquipped(category, productId);

    // ----- Write API -----
    public void SetCoins(int value)
    {
        _data.CoinsCollected = Mathf.Max(0, value);
        Save();
        OnCurrencyChanged?.Invoke();
    }

    public void AddCoins(int coinsToAdd) => SetCoins(_data.CoinsCollected + coinsToAdd);

    public void SetRecord(int distance)
    {
        _data.RecordDistance = Mathf.Max(_data.RecordDistance, distance);
        Save();
    }

    public void AddOwned(ProductCategory category, string productId)
    {
        _data.AddOwnedProduct(category, productId);
        Save();
        OnInventoryChanged?.Invoke(category, productId);
    }

    public void Equip(ProductCategory category, string productId)
    {
        _data.Equip(category, productId);
        Save();
        OnEquippedChanged?.Invoke(category, productId);
    }

    //should move this to another manager
    public void Preview(ProductCategory category, string productId)
    {
        OnPreviewItem?.Invoke(category, productId);
    }

    public void Unequip(ProductCategory category)
    {
        OnRevertItem?.Invoke(category);
    }
}
