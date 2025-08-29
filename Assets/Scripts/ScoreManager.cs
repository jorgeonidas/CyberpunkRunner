using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnCoinPickedEvent;
    [Header("Events to Trigger")]
    [SerializeField] CoinCollectedEvent _coinCollectedEvent;
    [Header("Events to Listen")]
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    int _coinsCollected;
    public int CoinsCollected => _coinsCollected;

    private void Start()
    {
        _coinsCollected = 0;
        UpdateScoreText();
    }
    private void OnEnable()
    {
        OnCoinPickedEvent += AddToCollectedCoins;
    }

    private void OnDisable()
    {
        OnCoinPickedEvent -= AddToCollectedCoins;
    }

    public void SaveCoinsCollected()
    {
        UserDataServiceSO.Instance.AddCoins(CoinsCollected);
    }

    private void AddToCollectedCoins(int scoreToAdd)
    {
        _coinsCollected += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
       _coinCollectedEvent.Raise(CoinsCollected);
    }
}
