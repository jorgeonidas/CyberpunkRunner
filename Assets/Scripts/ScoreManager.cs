using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnAddScoreEvent;
    [SerializeField] ScoreChangedEvent _scoreChangedEvent;
    int _currentScore;
    private void Start()
    {
        _currentScore = 0;
        UpdateScoreText();
    }
    private void OnEnable()
    {
        OnAddScoreEvent += AddScore;
    }

    private void OnDisable()
    {

        OnAddScoreEvent -= AddScore;
    }

    private void AddScore(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
       _scoreChangedEvent.Raise(_currentScore);
    }
}
