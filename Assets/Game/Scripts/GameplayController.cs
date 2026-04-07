using System;
using System.Linq;
using UnityEngine;

public class GameplayController
{
    private const int MaxBufferSize = 150;
    private const int TrimThreshold = 50;

    private readonly EnemyController _enemyController;
    private readonly InputController _inputController;
    private readonly PlayerController _playerController;
    private readonly BattleController _battleController;
    private readonly InputReader _inputReader;

    public GameplayController(InputController inputController, PlayerController playerController, EnemyController enemyController, BattleController battleController, InputReader inputReader)
    {
        _inputController = inputController;
        _playerController = playerController;
        _enemyController = enemyController;
        _battleController = battleController;
        _inputReader = inputReader;

        Subscribe();
    }

    public void Start()
    {
        _inputController.Enable();
        _inputReader.Clear();

        var character = new PlayerCharacter();
        _playerController.Spawn(character);

        _battleController.Spawn();
    }

    public void Subscribe()
    {
        if (_inputController == null) return;

        _inputController.TextInputted += OnTextInputted;
    }

    public void Unsubscribe()
    {
        _inputController.TextInputted -= OnTextInputted;
    }

    private void OnTextInputted(char inputtedChar)
    {
        TryTrimBuffer();

        _inputReader.Append(inputtedChar);

        string currentBuffer = _inputReader.ToString();

        var enemiesToShoot = _enemyController.Enemies
        .Where(enemy =>
        {
            int index = currentBuffer.IndexOf(enemy.Key.Sentence, enemy.Key.StartIndex);
            return index >= enemy.Key.StartIndex && index > enemy.Key.LastMatchIndex;
        })
        .ToHashSet();

        if (enemiesToShoot.Count == 0)
        {
            Debug.Log($"No matches");
            return;
        }

        var startShootPosition = _playerController.PlayerView.transform.position;
        foreach (var enemy in enemiesToShoot)
        {
            Debug.Log($"Match: {enemy.Key.Sentence}");
            _battleController.Shoot(startShootPosition, enemy.Value);
            enemy.Key.LastMatchIndex = _inputReader.LastIndex;
        }
    }

    private void TryTrimBuffer()
    {
        var inputLength = _inputReader.InputLength;
        if (inputLength <= MaxBufferSize) return;

        int trimAmount = inputLength - TrimThreshold;
        string newBuffer = _inputReader.Trim(trimAmount, TrimThreshold);

        _inputReader.Clear();
        _inputReader.Append(newBuffer);

        foreach (var enemy in _enemyController.Enemies)
        {
            enemy.Key.MoveIndexes(trimAmount);
        }

        Debug.Log($"Buffer trimmed from {inputLength + trimAmount} to {TrimThreshold}");
        Debug.Log($"Buffer: {newBuffer}");
    }
}