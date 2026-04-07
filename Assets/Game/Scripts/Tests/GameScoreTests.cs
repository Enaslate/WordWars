using NUnit.Framework;

public class GameScoreTests
{
    [Test]
    public void Correct_IncreasesStreakAndTotalChars()
    {
        var score = new GameScore();
        score.CorrectWord(1);
        Assert.AreEqual(1, score.CurrentStreak);
        Assert.AreEqual(1, score.BestStreak);
    }

    [Test]
    public void Mistake_ResetsStreakAndIncrementsErrors()
    {
        var score = new GameScore();
        score.CorrectLetter();
        score.Mistake();
        Assert.AreEqual(0, score.CurrentStreak);
        Assert.AreEqual(1, score.ErrorCount);
        Assert.AreEqual(1, score.TotalCharsTyped);
        Assert.AreEqual(2, score.TotalCharsAttempted);
        Assert.AreEqual(2, score.TotalCharsAttempted);
        Assert.AreEqual(0, score.EnemiesKilled);
    }

    [Test]
    public void BestStreak_RemembersMax()
    {
        var score = new GameScore();
        score.CorrectWord(2);
        score.Mistake();
        score.CorrectWord(1);
        Assert.AreEqual(2, score.BestStreak);
    }

    [Test]
    public void OnEnemyKilled_AddsScoreAndCount()
    {
        var score = new GameScore();
        score.OnEnemyKilled(100);
        Assert.AreEqual(100, score.Score);
        Assert.AreEqual(1, score.EnemiesKilled);
    }

    [Test]
    public void Accuracy_CalculatesCorrectly()
    {
        var score = new GameScore();
        score.TotalCharsTyped = 8;
        score.TotalCharsAttempted = 10;
        Assert.AreEqual(80f, score.Accuracy);
    }

    [Test]
    public void Accuracy_WhenNoAttempts_Returns100()
    {
        var score = new GameScore();
        Assert.AreEqual(100f, score.Accuracy);
    }
}
