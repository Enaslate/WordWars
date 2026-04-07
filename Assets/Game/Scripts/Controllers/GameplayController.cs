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
    private readonly GameScore _gameScore;

    public GameplayController(
        InputController inputController,
        PlayerController playerController,
        EnemyController enemyController,
        BattleController battleController,
        InputReader inputReader,
        GameScore gameScore)
    {
        _inputController = inputController;
        _playerController = playerController;
        _enemyController = enemyController;
        _battleController = battleController;
        _inputReader = inputReader;
        _gameScore = gameScore;
    }

    public void Start()
    {
        Unsubscribe();
        Subscribe();

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
        if (_inputController == null) return;
        
        _inputController.TextInputted -= OnTextInputted;
    }

    private void OnTextInputted(char inputtedChar)
    {
        TryTrimBuffer();

        string oldBuffer = _inputReader.ToString();
        _inputReader.Append(inputtedChar);
        string currentBuffer = _inputReader.ToString();

        bool anyProgress = false;
        foreach (var enemy in _enemyController.Enemies)
        {
            int oldMatchLen = GetMatchLength(oldBuffer, enemy.Key.StartIndex, enemy.Key.Sentence);
            int newMatchLen = GetMatchLength(currentBuffer, enemy.Key.StartIndex, enemy.Key.Sentence);
            if (newMatchLen > oldMatchLen) anyProgress = true;
            enemy.Value.UpdateHighlight(currentBuffer);
        }

        if (anyProgress)
            _gameScore.Correct();
        else
            _gameScore.Mistake();

        var enemiesToShoot = _enemyController.Enemies
            .Where(enemy =>
            {
                int index = currentBuffer.IndexOf(enemy.Key.Sentence, enemy.Key.StartIndex);
                return index >= enemy.Key.StartIndex && index > enemy.Key.LastMatchIndex;
            })
            .ToHashSet();

        if (enemiesToShoot.Count > 0)
        {
            var startShootPosition = _playerController.PlayerView.transform.position;
            foreach (var enemy in enemiesToShoot)
            {
                Debug.Log($"Match: {enemy.Key.Sentence}");
                _battleController.Shoot(startShootPosition, enemy.Value);
                enemy.Key.LastMatchIndex = _inputReader.LastIndex;
            }
        }
    }

    private int GetMatchLength(string buffer, int startIndex, string word)
    {
        if (startIndex >= buffer.Length) return 0;
        int maxLen = Mathf.Min(buffer.Length - startIndex, word.Length);

        for (int i = 0; i < maxLen; i++)
            if (buffer[startIndex + i] != word[i]) return i;

        return maxLen;
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