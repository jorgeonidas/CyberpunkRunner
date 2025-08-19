using System;
using UnityEditor;
using UnityEngine;

public static class PlayerDataManager
{
    private static UserData currentUserData;

    public static void Initialize()
    {
        currentUserData = SaveSystem.LoadUserData();
        Debug.Log("PlayerDataManager inicializado.");
    }

    public static int GetRecordDistance()
    {
        return currentUserData.recordDistance;
    }

    public static void SetRecordDistance(int newRecordDistance)
    {
        currentUserData.recordDistance = newRecordDistance;
        Debug.Log("Record Distance actualizado a: " + newRecordDistance);
    }

    public static int GetCoins()
    {
        return currentUserData.coinsCollected;
    }

    public static void AddCoins(int coinsToAdd)
    {
        SetCoins(GetCoins() + coinsToAdd);
    }

    public static void SetCoins(int newCoinsCollected)
    {
        currentUserData.coinsCollected = newCoinsCollected;
        Debug.Log("Coins Collected actualizado a: " + newCoinsCollected);
    }

    public static void SaveData()
    {
        SaveSystem.SaveUserData(currentUserData);
        Debug.Log("Datos del jugador guardados.");
    }
}
