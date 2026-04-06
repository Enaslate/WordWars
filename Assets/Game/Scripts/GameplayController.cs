using System.Linq;
using System.Text;
using UnityEngine;

public class GameplayController
{
    private const int MaxBufferSize = 150;
    private const int TrimThreshold = 50;

    private readonly EnemyController _enemyController;
    private readonly InputController _inputController;

    private readonly StringBuilder _currentInput;

    private string[] Sentences = new string[]
    {
        "вес",
        "веселье",
        "ель",
        "навес",
    };

    public GameplayController(InputController inputController, EnemyController enemyController)
    {
        _currentInput = new StringBuilder();
        _inputController = inputController;
        _enemyController = enemyController;

        Subscribe();
    }

    public void Start()
    {
        _inputController.Enable();
        _currentInput.Clear();

        Spawn();
    }

    public void Spawn()
    {
        for (int i = _enemyController.Enemies.Count; i < 25; i++)
        {
            var next = Random.Range(0, Sentences.Length - 1);
            var enemy = new Enemy(_currentInput.Length, Sentences[next]);
            _enemyController.Spawn(enemy);
        }
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

        _currentInput.Append(inputtedChar);

        string currentBuffer = _currentInput.ToString();

        var enemiesToShoot = _enemyController.Enemies
            .Where(enemy =>
                currentBuffer.IndexOf(enemy.Sentence, enemy.StartIndex) >= enemy.StartIndex)
            .ToList();

        if (enemiesToShoot.Count == 0)
        {
            Debug.Log($"No matches");
            return;
        }

        foreach (var enemy in enemiesToShoot)
        {
            Debug.Log($"Match: {enemy.Sentence}");
            enemy.Damage(100f);
        }

        Spawn();
    }


    private void TryTrimBuffer()
    {
        if (_currentInput.Length <= MaxBufferSize) return;

        int trimAmount = _currentInput.Length - TrimThreshold;
        string newBuffer = _currentInput.ToString(trimAmount, TrimThreshold);

        _currentInput.Clear();
        _currentInput.Append(newBuffer);

        foreach (var enemy in _enemyController.Enemies)
        {
            enemy.MoveIndex(trimAmount);
        }

        Debug.Log($"Buffer trimmed from {_currentInput.Length + trimAmount} to {TrimThreshold}");
        Debug.Log($"Buffer: {newBuffer}");
    }
}