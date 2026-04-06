using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    private readonly EnemyPoolController _enemyPool;

    private Dictionary<Enemy, EnemyView> _enemies = new();
    public IReadOnlyDictionary<Enemy, EnemyView> Enemies => _enemies;

    public EnemyController(EnemyPoolController enemyPoolController)
    {
        _enemyPool = enemyPoolController;
    }

    public void Spawn(Enemy enemy)
    {
        var insideCircle = UnityEngine.Random.insideUnitCircle * 10;

        var position = new Vector3(insideCircle.x, insideCircle.y, 0);
        var view = _enemyPool.Get();
        
        view.Setup(position, enemy);
        _enemies.Add(enemy, view);
    }

    public void Despawn(Enemy enemy)
    {
        if (!_enemies.TryGetValue(enemy, out var view))
            return;

        _enemies.Remove(enemy);
        _enemyPool.Release(view);
    }
}