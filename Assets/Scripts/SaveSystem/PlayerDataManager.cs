using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PlayerDataManager
{
    public static Action<ProductCategory, string> OnItemEquipped;
    public static event Action OnCurrencyChanged;
    private static UserData _currentUserData;
    public static void Initialize()
    {
        _currentUserData = SaveSystem.LoadUserData();
        Debug.Log($"PlayerDataManager inicializado.");
    }

    public static int GetRecordDistance()
    {
        return _currentUserData.RecordDistance;
    }

    public static void SetRecordDistance(int newRecordDistance)
    {
        _currentUserData.RecordDistance = newRecordDistance;
        Debug.Log("Record Distance actualizado a: " + newRecordDistance);
    }

    public static int GetCoins()
    {
        return _currentUserData.CoinsCollected;
    }

    public static UserGameSettings GetUserGameSettings()
    {
        Debug.Log($"user game settings? {_currentUserData != null} {_currentUserData.userGameSettings != null}");
        return _currentUserData.userGameSettings;
    }

    public static void AddCoins(int coinsToAdd)
    {
        SetCoins(GetCoins() + coinsToAdd);
    }

    public static void SetCoins(int newCoinsCollected)
    {
        _currentUserData.CoinsCollected = newCoinsCollected;
        Debug.Log("Coins Collected actualizado a: " + newCoinsCollected);
    }

    public static void SetUserGameSettings(UserGameSettings userGameSettings)
    {
        _currentUserData.userGameSettings = userGameSettings;
        Debug.Log("User Game Settings actualizado.");
    }

    public static List<string> GetOwnedProductsByCategory(ProductCategory category)
    {
        return _currentUserData.GetOwnedProductsByCategory(category);
    }

    public static bool CheckIfProductIsOwned(ProductCategory category, string productId)
    {
        return _currentUserData.CheckIfProductIsOwned(category, productId);
    }

    public static bool CheckIfProductIsEquipped(ProductCategory category, string productId)
    {
        return _currentUserData.CheckIfProductIsEquipped(category, productId);
    }


    public static void AddOwnedProduct(ProductCategory category, string productId)
    {
        _currentUserData.AddOwnedProduct(category, productId);
    }

    public static void Equip(ProductCategory category, string productId)
    {
        _currentUserData.Equip(category, productId);
        SaveData();
        OnItemEquipped?.Invoke(category, productId);
    }


    public static void SaveData()
    {
        SaveSystem.SaveUserData(_currentUserData);
        Debug.Log("Datos del jugador guardados.");
        OnCurrencyChanged?.Invoke();
    }
}
