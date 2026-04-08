using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController
{
    private readonly EnemyPoolController _enemyPool;
    private readonly SentencePoolController _sentencePool;
    private readonly PlayerController _playerController;

    private Dictionary<Enemy, EnemyView> _enemies = new();
    public IReadOnlyDictionary<Enemy, EnemyView> Enemies => _enemies;

    public EnemyController(EnemyPoolController enemyPoolController, SentencePoolController sentencePool, PlayerController playerController)
    {
        _enemyPool = enemyPoolController;
        _sentencePool = sentencePool;
        _playerController = playerController;
    }

    public void Spawn(Enemy enemy)
    {
        var outsideCircle = UnityEngine.Random.insideUnitCircle.normalized * Random.Range(10f, 15f);

        var position = new Vector3(outsideCircle.x, outsideCircle.y, 0);

        var view = _enemyPool.Get();
        var sentenceView = _sentencePool.Get();
        
        view.Setup(position, enemy, sentenceView, _playerController.PlayerView);
        _enemies.Add(enemy, view);
    }

    public void Despawn(Enemy enemy)
    {
        if (!_enemies.TryGetValue(enemy, out var view))
            return;

        _sentencePool.Release(view.SentenceView);
        _enemyPool.Release(view);
        _enemies.Remove(enemy);
    }

    public void DespawnAll()
    {
        foreach (var enemy in _enemies.Keys.ToList())
        {
            Despawn(enemy);
        }
    }
}