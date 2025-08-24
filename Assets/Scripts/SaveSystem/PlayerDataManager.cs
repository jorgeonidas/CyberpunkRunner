using System;
using UnityEditor;
using UnityEngine;

public static class PlayerDataManager
{
    public static event Action OnCurrencyChanged;
    private static UserData _currentUserData;
    public static void Initialize()
    {
        _currentUserData = SaveSystem.LoadUserData();
        Debug.Log($"PlayerDataManager inicializado.");
    }

    public static int GetRecordDistance()
    {
        return _currentUserData.recordDistance;
    }

    public static void SetRecordDistance(int newRecordDistance)
    {
        _currentUserData.recordDistance = newRecordDistance;
        Debug.Log("Record Distance actualizado a: " + newRecordDistance);
    }

    public static int GetCoins()
    {
        return _currentUserData.coinsCollected;
    }

    public static UserGameSettings GetUserGameSettings()
    {
        return _currentUserData.userGameSettings;
    }

    public static void AddCoins(int coinsToAdd)
    {
        SetCoins(GetCoins() + coinsToAdd);
    }

    public static void SetCoins(int newCoinsCollected)
    {
        _currentUserData.coinsCollected = newCoinsCollected;
        Debug.Log("Coins Collected actualizado a: " + newCoinsCollected);
    }

    public static void SetUserGameSettings(UserGameSettings userGameSettings)
    {
        _currentUserData.userGameSettings = userGameSettings;
        Debug.Log("User Game Settings actualizado.");
    }

    public static void SaveData()
    {
        SaveSystem.SaveUserData(_currentUserData);
        Debug.Log("Datos del jugador guardados.");
        OnCurrencyChanged?.Invoke();
    }
}
