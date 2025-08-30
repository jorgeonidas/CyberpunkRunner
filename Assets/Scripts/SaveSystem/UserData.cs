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
    private Dictionary<ProductCategory, List<string>> _ownedProducts;
    private Dictionary<ProductCategory, string> _equippedProducts;

    public int CoinsCollected { get => _coinsCollected; set => _coinsCollected = value; }
    public int RecordDistance { get => _recordDistance; set => _recordDistance = value; }
    public Dictionary<ProductCategory, List<string>> OwnedProducts { get => _ownedProducts; set => _ownedProducts = value; }
    //one equpped product per category
    public Dictionary<ProductCategory, string> EquippedProducts { get => _equippedProducts; set => _equippedProducts = value; }

    public UserData()
    {
        RecordDistance = 0;
        _coinsCollected = 0;
        _ownedProducts = new SerializedDictionary<ProductCategory, List<string>>();
        _equippedProducts = new SerializedDictionary<ProductCategory, string>();
        userGameSettings = new UserGameSettings();

        // Iterar sobre todas las categorías de productos para inicializarlas.
        foreach (ProductCategory category in Enum.GetValues(typeof(ProductCategory)))
        {
            // Inicializar los productos poseídos con el item por defecto.
            _ownedProducts[category] = new List<string> { StringConstants.DefaultItem };

            // Inicializar los productos equipados con el item por defecto.
            _equippedProducts[category] = StringConstants.DefaultItem;
        }
    }

    public List<string> GetOwnedProductsByCategory(ProductCategory category)
    {
        if (_ownedProducts.TryGetValue(category, out var productList))
        {
            return productList;
        }
        return new List<string> { StringConstants.DefaultItem };
    }

    public void AddOwnedProduct(ProductCategory category, string productId)
    {
        if (!_ownedProducts.ContainsKey(category))
        {
            _ownedProducts[category] = new List<string>();
        }

        _ownedProducts[category].Add(productId);
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
        return GetOwnedProductsByCategory(category).Contains(productId);
    }

    public bool CheckIfProductIsEquipped(ProductCategory category, string productId)
    {
        return GetEquippedProductInCategory(category) == productId;
    }
}

[System.Serializable]
public class UserGameSettings
{
    public float musicVolume;
    public float sfxVolume;
    public UserGameSettings()
    {
        musicVolume = 1f;
        sfxVolume = 1f;
    }
}
