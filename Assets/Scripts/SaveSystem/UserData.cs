using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[System.Serializable]
public class UserData
{
    private int _recordDistance;
    private int _coinsCollected;
    public UserGameSettings userGameSettings;
    private Dictionary<ProductCategory, List<string>> _onwedProducts;
    private Dictionary<ProductCategory, string> _equippedProducts;

    public int CoinsCollected { get => _coinsCollected; set => _coinsCollected = value; }
    public int RecordDistance { get => _recordDistance; set => _recordDistance = value; }
    public Dictionary<ProductCategory, List<string>> OnwedProducts { get => _onwedProducts; set => _onwedProducts = value; }
    //one equpped product per category
    public Dictionary<ProductCategory, string> EquippedProducts { get => _equippedProducts; set => _equippedProducts = value; }

    public UserData()
    {
        RecordDistance = 0;
        _coinsCollected = 0;
        _onwedProducts = new SerializedDictionary<ProductCategory, List<string>>();
        EquippedProducts = new SerializedDictionary<ProductCategory, string>();
        userGameSettings = new UserGameSettings();
    }

    public List<string> GetOwnedProductsByCategory(ProductCategory category)
    {
        if (!_onwedProducts.ContainsKey(category))
        {
            return new List<string>() { StringConstants.DefaultItem };
        }

        return _onwedProducts[category];
    }

    public void AddOwnedProduct(ProductCategory category, string productId)
    {
        if (!_onwedProducts.ContainsKey(category))
        {
            _onwedProducts[category] = new List<string>();
        }

        _onwedProducts[category].Add(productId);
    }

    public void Equip(ProductCategory category, string productId)
    {
        EquippedProducts[category] = productId;
    }

    public string GetEquippedProductInCategory(ProductCategory category)
    {
        if (EquippedProducts.TryGetValue(category, out string equippedItem))
        {
            return equippedItem;
        }
        return StringConstants.DefaultItem;
    }

    public bool CheckIfProductIsOwned(ProductCategory category, string productId)
    {
        var ownedInCategory = GetOwnedProductsByCategory(category);
        if (ownedInCategory.Contains(productId))
        {
            return true;
        }
        return false;
    }

    public bool CheckIfProductIsEquipped(ProductCategory category, string productId)
    {
        if (GetEquippedProductInCategory(category) == productId)
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class UserGameSettings
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
}