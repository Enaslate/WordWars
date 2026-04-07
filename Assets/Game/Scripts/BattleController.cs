using UnityEngine;

public class BattleController
{
    private readonly EnemyController _enemyController;
    private readonly ProjectilePoolController _projectilePool;
    private readonly InputReader _inputReader;

    private string[] Sentences = new string[]
    {
        "вес",
        "веселье",
        "ель",
        "навес",
    };

    public BattleController(EnemyController enemyController, ProjectilePoolController projectilePool, InputReader inputReader)
    {
        _enemyController = enemyController;
        _projectilePool = projectilePool;
        
        _inputReader = inputReader;
    }

    public void Spawn()
    {
        for (int i = _enemyController.Enemies.Count; i < 25; i++)
        {
            var next = UnityEngine.Random.Range(0, Sentences.Length - 1);
            var enemy = new Enemy(_inputReader.InputLength, Sentences[next]);
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

    public void OnDied(Enemy enemy)
    {
        enemy.Died -= OnDied;
        _enemyController.Despawn(enemy);
        Spawn();
    }
}