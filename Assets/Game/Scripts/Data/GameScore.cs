using System;
using UnityEngine;

[Serializable]
public class GameScore
{
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

    public float WPM => (TotalCharsTyped / 5f) / (GameTimeSeconds / 60f);

    public void Correct()
    {
        TotalCharsTyped++;
        TotalCharsAttempted++;
        CurrentStreak++;
        if (CurrentStreak > BestStreak) BestStreak = CurrentStreak;
    }

    public void Mistake()
    {
        TotalCharsAttempted++;
        ErrorCount++;
        CurrentStreak = 0;
    }

    public void OnEnemyKilled(int basePoints, float multiplier)
    {
        int points = Mathf.RoundToInt(basePoints * multiplier);
        Score += points;
        EnemiesKilled++;
    }
}