using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action<int> OnScoreChanged;
    [SerializeField] TextMeshProUGUI _scoreText;
    int _currentScore;
    private void Start()
    {
        _currentScore = 0;
        UpdateScoreText();
    }
    private void OnEnable()
    {
        OnScoreChanged += AddScore;
    }

    private void OnDisable()
    {

        OnScoreChanged -= AddScore;
    }

    private void AddScore(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = $"SCORE: {_currentScore}";
    }
}
