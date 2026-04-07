using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    private readonly EnemyPoolController _enemyPool;
    private readonly SentencePoolController _sentencePool;

    private Dictionary<Enemy, EnemyView> _enemies = new();
    public IReadOnlyDictionary<Enemy, EnemyView> Enemies => _enemies;

    public EnemyController(EnemyPoolController enemyPoolController, SentencePoolController sentencePool)
    {
        _enemyPool = enemyPoolController;
        _sentencePool = sentencePool;
    }

    public void Spawn(Enemy enemy)
    {
        var insideCircle = UnityEngine.Random.insideUnitCircle * 10;

        var position = new Vector3(insideCircle.x, insideCircle.y, 0);

        var view = _enemyPool.Get();
        var sentenceView = _sentencePool.Get();
        
        view.Setup(position, enemy, sentenceView);
        _enemies.Add(enemy, view);
    }

    public void Despawn(Enemy enemy)
    {
        if (!_enemies.TryGetValue(enemy, out var view))
            return;

        _enemies.Remove(enemy);
        _sentencePool.Release(view.SentenceView);
        _enemyPool.Release(view);
    }
}