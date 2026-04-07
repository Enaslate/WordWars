using UnityEngine;

public class BattleController
{
    private readonly EnemyController _enemyController;
    private readonly ProjectilePoolController _projectilePool;
    private readonly TextInputProcessor _inputProcessor;
    private readonly GameScore _gameScore;

    private string[] Sentences = new string[]
    {
        "вес",
        "веселье",
        "ель",
        "навес",
    };

    public BattleController(EnemyController enemyController, ProjectilePoolController projectilePool, TextInputProcessor inputReader, GameScore gameScore)
    {
        _enemyController = enemyController;
        _projectilePool = projectilePool;

        _inputProcessor = inputReader;
        _gameScore = gameScore;
    }

    public void Spawn()
    {
        for (int i = _enemyController.Enemies.Count; i < 3; i++)
        {
            var next = UnityEngine.Random.Range(0, Sentences.Length - 1);
            var enemy = new Enemy(_inputProcessor.InputLength, Sentences[next]);
            _enemyController.Spawn(enemy);
            enemy.Died += OnDied;
        }
    }

    public void Shoot(Vector3 startPosition, EnemyView target)
    {
        var projectileView = _projectilePool.Get();
        projectileView.Setup(startPosition, target, 100, _projectilePool);

        if (projectileView == null) return;
    }

    public void OnDied(Character character)
    {
        character.Died -= OnDied;
        _gameScore.OnEnemyKilled(1);
        _enemyController.Despawn(character as Enemy);
        Spawn();
    }
}