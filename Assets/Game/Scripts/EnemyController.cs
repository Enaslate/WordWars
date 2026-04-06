using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    private readonly EnemyPoolController _enemyPoolController;

    private List<Enemy> _enemies = new();
    public IReadOnlyList<Enemy> Enemies => _enemies;

    public EnemyController(EnemyPoolController enemyPoolController)
    {
        _enemyPoolController = enemyPoolController;
    }

    public void Spawn(Enemy enemy)
    {
        var insideCircle = UnityEngine.Random.insideUnitCircle * 3;

        var position = new Vector3(insideCircle.x, insideCircle.y, 0);
        _enemyPoolController.Spawn(enemy, position);
        _enemies.Add(enemy);
        enemy.Died += OnDied;
    }

    public void OnDied(Enemy enemy)
    {
        enemy.Died -= OnDied;
        _enemyPoolController.Despawn(enemy);
        _enemies.Remove(enemy);
    }
}