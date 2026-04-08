using System;
using TMPro;
using UnityEngine;

public class GameScoreView : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] private Transform _window;
    [Header("Fields")]
    [SerializeField] private TMP_Text _scoreValue;
    [SerializeField] private TMP_Text _bestStreakValue;
    [SerializeField] private TMP_Text _errorCountValue;
    [SerializeField] private TMP_Text _totalCharTypedValue;
    [SerializeField] private TMP_Text _totalCharAttemptedValue;
    [SerializeField] private TMP_Text _enemiesKilledValue;
    [SerializeField] private TMP_Text _gameTimeSecondsValue;
    [SerializeField] private TMP_Text _accuracyValue;
    [SerializeField] private TMP_Text _wordPerMinuteValue;

    private GameScore _gameScore;

    public void Setup(GameScore gameScore)
    {
        _gameScore = gameScore;
        SetValues(_gameScore);
    }

    public void SetValues(GameScore gameScore)
    {
        _scoreValue.text = _gameScore.Score.ToString("N0");
        _bestStreakValue.text = _gameScore.BestStreak.ToString("N0");
        _errorCountValue.text = _gameScore.ErrorCount.ToString("N0");
        _totalCharTypedValue.text = _gameScore.TotalCharsTyped.ToString("N0");
        _totalCharAttemptedValue.text = _gameScore.TotalCharsAttempted.ToString("N0");
        _enemiesKilledValue.text = _gameScore.EnemiesKilled.ToString("N0");
        _gameTimeSecondsValue.text = _gameScore.GameTimeSeconds.ToString("N0") + "сек.";
        _accuracyValue.text = _gameScore.Accuracy.ToString("N0") + "%";
        _wordPerMinuteValue.text = _gameScore.WPM.ToString("N0");
    }

    public void Show()
    {
        SetValues(_gameScore);
        _window.gameObject?.SetActive(true);
    }

    public void Hide()
    {
        _window.gameObject?.SetActive(false);
    }
}