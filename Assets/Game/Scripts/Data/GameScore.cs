using System;
using UnityEngine;

[Serializable]
public class GameScore
{
    public float Multiplier = 1f;

    public int Score = 0;
    public int BestStreak = 0;
    public int CurrentStreak = 0;
    public int ErrorCount = 0;
    public int TotalCharsTyped = 0;
    public int TotalCharsAttempted = 0;
    public int EnemiesKilled = 0;
    public float GameTimeSeconds = 0f;

    public float Accuracy => TotalCharsAttempted == 0 ? 100f :
        (float)TotalCharsTyped / TotalCharsAttempted * 100f;

    public float WPM => GameTimeSeconds > 0 ? (TotalCharsTyped / 5f) / (GameTimeSeconds / 60f) : 0f;

    public void CorrectWord(int count)
    {
        CurrentStreak += count;
        if (CurrentStreak > BestStreak) BestStreak = CurrentStreak;
    }

    public void CorrectLetter()
    {
        TotalCharsTyped++;
        TotalCharsAttempted++;
    }

    public void Mistake()
    {
        TotalCharsAttempted++;
        ErrorCount++;
        CurrentStreak = 0;
    }

    public void OnEnemyKilled(int basePoints)
    {
        Multiplier = GetMultiplier();
        int points = Mathf.RoundToInt(basePoints * Multiplier);
        Score += points;
        EnemiesKilled++;
    }

    public void UpdateGameScore(float delta)
    {
        GameTimeSeconds += delta;
    }

    private float GetMultiplier()
    {
        if (CurrentStreak <= 1)
            return 1;

        return 1 + (CurrentStreak / Accuracy * 10);
    }

    public void Reset()
    {
        Multiplier = 1f;

        Score = 0;
        BestStreak = 0;
        CurrentStreak = 0;
        ErrorCount = 0;
        TotalCharsTyped = 0;
        TotalCharsAttempted = 0;
        EnemiesKilled = 0;
        GameTimeSeconds = 0f;
    }
}