using System.Linq;
using UnityEngine;

public class GameplayController
{
    private readonly EnemyController _enemyController;
    private readonly InputController _inputController;
    private readonly PlayerController _playerController;
    private readonly BattleController _battleController;
    private readonly TextInputProcessor _inputProcessor;
    private readonly GameScoreController _gameScoreController;
    private readonly AudioController _audioController;

    public GameplayController(
        InputController inputController,
        PlayerController playerController,
        EnemyController enemyController,
        BattleController battleController,
        TextInputProcessor inputReader,
        GameScoreController gameScoreController,
        AudioController audioController)
    {
        _inputController = inputController;
        _playerController = playerController;
        _enemyController = enemyController;
        _battleController = battleController;
        _inputProcessor = inputReader;
        _gameScoreController = gameScoreController;
        _audioController = audioController;
    }

    public void Start()
    {
        Unsubscribe();
        Subscribe();

        _inputController.Enable();
        _inputProcessor.Clear();

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
        _audioController.PlayClick();
        var enemies = _enemyController.Enemies;
        var enemiesToShoot = _inputProcessor.Process(inputtedChar, enemies, _gameScoreController.Score);

        if (enemiesToShoot.Count > 0)
        {
            var startShootPosition = _playerController.PlayerView.transform.position;
            foreach (var enemy in enemiesToShoot)
            {
                Debug.Log($"Match: {enemy.Key.Sentence}");
                _battleController.Shoot(startShootPosition, enemy.Value);
            }
        }
    }
}