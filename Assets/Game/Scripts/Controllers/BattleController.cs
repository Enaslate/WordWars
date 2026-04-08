using UnityEngine;

public class BattleController
{
    private readonly EnemyController _enemyController;
    private readonly ProjectilePoolController _projectilePool;
    private readonly TextInputProcessor _inputProcessor;
    private readonly GameScoreController _gameScoreController;

    public BattleController(EnemyController enemyController, ProjectilePoolController projectilePool, TextInputProcessor inputReader, GameScoreController gameScoreController)
    {
        _enemyController = enemyController;
        _projectilePool = projectilePool;

        _inputProcessor = inputReader;
        _gameScoreController = gameScoreController;
    }

    public void Spawn()
    {
        for (int i = _enemyController.Enemies.Count; i < 3; i++)
        {
            var next = UnityEngine.Random.Range(0, SentenceStorageHelper.Sentences.Length - 1);
            var enemy = new Enemy(_inputProcessor.InputLength, SentenceStorageHelper.Sentences[next]);
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

        if (character is not Enemy enemy) return;

        if (!enemy.IsSuicideAttack)
            _gameScoreController.Score.OnEnemyKilled(10);

        _enemyController.Despawn(enemy);
        Spawn();
    }
}