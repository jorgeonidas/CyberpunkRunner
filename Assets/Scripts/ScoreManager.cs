using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnCoinPickedEvent;
    [Header("Events to Trigger")]
    [SerializeField] CoinCollectedEvent _coinCollectedEvent;
    [Header("Events to Listen")]
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    int _coinsCollected;
    private void Start()
    {
        _coinsCollected = 0;
        UpdateScoreText();
    }
    private void OnEnable()
    {
        OnCoinPickedEvent += AddScore;
        _gameStateChangedEvent.OnEventRaised += OnGameStateChanged;
    }

    private void OnDisable()
    {
        OnCoinPickedEvent -= AddScore;
        _gameStateChangedEvent.OnEventRaised -= OnGameStateChanged; 
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.GameOver)
        {
            PlayerDataManager.AddCoins(_coinsCollected);
            PlayerDataManager.SetRecordDistance(GameManager.Instance.GetDistanceTravelled());
            //test save file
            PlayerDataManager.SaveData();

        }
    }

    private void AddScore(int scoreToAdd)
    {
        _coinsCollected += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
       _coinCollectedEvent.Raise(_coinsCollected);
    }
}
