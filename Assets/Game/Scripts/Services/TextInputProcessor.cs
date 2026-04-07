using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextInputProcessor
{
    private const int MaxBufferSize = 150;
    private const int TrimThreshold = 50;

    private readonly StringBuilder _currentInput;
    public int InputLength => _currentInput.Length;
    public int LastIndex => _currentInput.Length - 1;

    public TextInputProcessor()
    {
        _currentInput = new StringBuilder();
    }

    public void Clear()
    {
        _currentInput.Clear();
    }

    public HashSet<KeyValuePair<Enemy, EnemyView>> Process(char inputtedChar, IReadOnlyDictionary<Enemy, EnemyView> enemies, GameScore gameScore)
    {
        TryTrimBuffer(enemies);

        var oldBuffer = _currentInput.ToString();
        _currentInput.Append(inputtedChar);
        var currentBuffer = _currentInput.ToString();

        bool anyProgress = enemies.Any(enemy =>
        {
            var maxMatch = MatchHelper.GetMaxMatchLength(currentBuffer, enemy.Key.Sentence, enemy.Key.StartIndex);

            return maxMatch > 0;
        });

        foreach (var enemy in enemies)
        {
            enemy.Value.UpdateHighlight(currentBuffer);
        }

        if (anyProgress)
            gameScore.CorrectLetter();
        else
            gameScore.Mistake();

        var enemiesToShoot = enemies
            .Where(enemy =>
            {
                int index = currentBuffer.IndexOf(enemy.Key.Sentence, enemy.Key.StartIndex);
                return index >= enemy.Key.StartIndex && index > enemy.Key.LastMatchIndex;
            })
            .ToHashSet();

        foreach (var enemy in enemiesToShoot)
            enemy.Key.LastMatchIndex = LastIndex;

        var uniqueSentenceCount = enemiesToShoot
            .Select(enemy => enemy.Key.Sentence)
            .Distinct()
            .Count();

        if (uniqueSentenceCount > 0)
            gameScore.CorrectWord(uniqueSentenceCount);

        return enemiesToShoot;
    }

    private void TryTrimBuffer(IReadOnlyDictionary<Enemy, EnemyView> enemies)
    {
        var inputLength = InputLength;
        if (inputLength <= MaxBufferSize) return;

        int trimAmount = inputLength - TrimThreshold;
        string newBuffer = _currentInput.ToString(trimAmount, TrimThreshold);

        Clear();
        _currentInput.Append(newBuffer);

        foreach (var enemy in enemies)
        {
            enemy.Key.MoveIndexes(trimAmount);
        }

        Debug.Log($"Buffer trimmed from {inputLength + trimAmount} to {TrimThreshold}");
        Debug.Log($"Buffer: {newBuffer}");
    }
}