using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Duplicated CoinsManager");
            Destroy(gameObject);
        }
        Instance = this;
    }

    private readonly List<CoinPickUp> _activeCoins = new List<CoinPickUp>();

    public void RegisterCoin(CoinPickUp coin)
    {
        _activeCoins.Add(coin);
        //Debug.Log($"Registered coin. Total active coins: {_activeCoins.Count}");
    }

    public void UnregisterCoin(CoinPickUp coin)
    {
        _activeCoins.Remove(coin);
        //Debug.Log($"Unregistered coin. Total active coins: {_activeCoins.Count}");
    }

    public List<CoinPickUp> GetActiveCoins()
    {
        return _activeCoins;
    }   
}
