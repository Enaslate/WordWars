using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TextInputProcessorTests
{
    private TextInputProcessor _processor;
    private GameScore _score;
    private Dictionary<Enemy, EnemyView> _enemies;

    [SetUp]
    public void Setup()
    {
        _processor = new TextInputProcessor();
        _score = new GameScore();
        _enemies = new Dictionary<Enemy, EnemyView>();
    }

    private void AddEnemy(string sentence, int startIndex = 0, int lastMatchIndex = -1)
    {
        var enemy = new Enemy(startIndex, sentence) { LastMatchIndex = lastMatchIndex };
        var gameObj = new GameObject();
        var view = gameObj.AddComponent<EnemyView>();
        _enemies[enemy] = view;
    }

    private void AssertScore(int expectedScore, int expectedStreak, int expectedBestStreak, int expectedErrors,
                             int expectedTyped, int expectedAttempted)
    {
        Assert.AreEqual(expectedScore, _score.Score);
        Assert.AreEqual(expectedStreak, _score.CurrentStreak);
        Assert.AreEqual(expectedBestStreak, _score.BestStreak);
        Assert.AreEqual(expectedErrors, _score.ErrorCount);
        Assert.AreEqual(expectedTyped, _score.TotalCharsTyped);
        Assert.AreEqual(expectedAttempted, _score.TotalCharsAttempted);
    }

    [Test]
    public void SingleWord_AllCorrect_StreakOnWordOnly()
    {
        AddEnemy("hello");

        _processor.Process('h', _enemies, _score);
        AssertScore(0, 0, 0, 0, 1, 1);
        _processor.Process('e', _enemies, _score);
        AssertScore(0, 0, 0, 0, 2, 2);
        _processor.Process('l', _enemies, _score);
        AssertScore(0, 0, 0, 0, 3, 3);
        _processor.Process('l', _enemies, _score);
        AssertScore(0, 0, 0, 0, 4, 4);
        _processor.Process('o', _enemies, _score);
        AssertScore(0, 1, 1, 0, 5, 5);
    }

    [Test]
    public void MultipleWords_ProgressOnAnyWord()
    {
        AddEnemy("hello");
        AddEnemy("world");

        _processor.Process('h', _enemies, _score);
        AssertScore(0, 0, 0, 0, 1, 1);

        _processor.Process('e', _enemies, _score);
        AssertScore(0, 0, 0, 0, 2, 2);

        _processor.Process('l', _enemies, _score);
        AssertScore(0, 0, 0, 0, 3, 3);

        _processor.Process('x', _enemies, _score);
        AssertScore(0, 0, 0, 1, 3, 4);

        _processor.Process('w', _enemies, _score);
        AssertScore(0, 0, 0, 1, 4, 5);
    }

    [Test]
    public void ErrorThenRecovery_StreakResets()
    {
        AddEnemy("unity");

        _processor.Process('u', _enemies, _score);
        _processor.Process('n', _enemies, _score);
        _processor.Process('i', _enemies, _score);
        AssertScore(0, 0, 0, 0, 3, 3);

        _processor.Process('x', _enemies, _score);
        AssertScore(0, 0, 0, 1, 3, 4);

        _processor.Process('u', _enemies, _score);
        AssertScore(0, 0, 0, 1, 4, 5);
        _processor.Process('n', _enemies, _score);
        AssertScore(0, 0, 0, 1, 5, 6);
        _processor.Process('i', _enemies, _score);
        AssertScore(0, 0, 0, 1, 6, 7);
        _processor.Process('t', _enemies, _score);
        AssertScore(0, 0, 0, 1, 7, 8);
        _processor.Process('y', _enemies, _score);
        AssertScore(0, 1, 1, 1, 8, 9);
    }

    [Test]
    public void PartialMatch_NoProgressWhenMistakeInsideWord()
    {
        AddEnemy("abcdef");

        _processor.Process('a', _enemies, _score);
        _processor.Process('b', _enemies, _score);
        _processor.Process('c', _enemies, _score);
        AssertScore(0, 0, 0, 0, 3, 3);

        _processor.Process('z', _enemies, _score);
        AssertScore(0, 0, 0, 1, 3, 4);

        _processor.Process('a', _enemies, _score);
        AssertScore(0, 0, 0, 1, 4, 5);
        _processor.Process('b', _enemies, _score);
        AssertScore(0, 0, 0, 1, 5, 6);
    }

    [Test]
    public void Shooting_WhenWordFullyMatched_ReturnsEnemy()
    {
        AddEnemy("fire");

        _processor.Process('f', _enemies, _score);
        _processor.Process('i', _enemies, _score);
        _processor.Process('r', _enemies, _score);
        var result = _processor.Process('e', _enemies, _score);

        Assert.AreEqual(1, result.Count());
    }

    [Test]
    public void NoShoot_WhenPartialMatch()
    {
        AddEnemy("shoot");

        var result = _processor.Process('s', _enemies, _score);
        Assert.IsEmpty(result);

        result = _processor.Process('h', _enemies, _score);
        Assert.IsEmpty(result);
    }

    [Test]
    public void NoShoot_WhenAlreadyShot()
    {
        AddEnemy("boom", lastMatchIndex: 3);

        var result = _processor.Process('b', _enemies, _score);
        Assert.IsEmpty(result);
    }

    [Test]
    public void ScoreAndStreakWithEnemyKill()
    {
        AddEnemy("kill");

        _processor.Process('k', _enemies, _score);
        _processor.Process('i', _enemies, _score);
        _processor.Process('l', _enemies, _score);
        var result = _processor.Process('l', _enemies, _score);

        foreach (var enemy in result)
            _score.OnEnemyKilled(100);

        Assert.AreEqual(100, _score.Score);
        Assert.AreEqual(1, _score.EnemiesKilled);
        Assert.AreEqual(1, _score.CurrentStreak);
        Assert.AreEqual(1, _score.BestStreak);
    }
}