using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPoolController : MonoBehaviour
{
    [SerializeField] private EnemyView _enemyPrefab;
    private ObjectPool<EnemyView> _enemyPool;
    private Dictionary<Enemy, EnemyView> _activeEnemies = new();

    private void Start()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is missing!", this);
            return;
        }

        _enemyPool = new ObjectPool<EnemyView>(
            createFunc: () => Instantiate(_enemyPrefab, transform),
            actionOnGet: view => view.gameObject.SetActive(true),
            actionOnRelease: view =>
            {
                view.Reset();
                view.gameObject.SetActive(false);
            },
            actionOnDestroy: (view) => Destroy(view.gameObject),
            defaultCapacity: 10,
            maxSize: 100);
    }

    private void OnDestroy()
    {
        _enemyPool?.Dispose();
        _activeEnemies.Clear();
    }

    public void Spawn(Enemy enemy, Vector3 position)
    {
        if (_enemyPool == null) return;

        var view = _enemyPool.Get();
        _activeEnemies[enemy] = view;
        view.Setup(position, enemy);
    }

    public void Despawn(Enemy enemy)
    {
        if (_enemyPool == null) return;

        if (_activeEnemies.TryGetValue(enemy, out var view))
        {
            _activeEnemies.Remove(enemy);
            _enemyPool.Release(view);
        }
    }

    public void DespawnAll()
    {
        foreach (var pair in _activeEnemies)
        {
            _enemyPool.Release(pair.Value);
        }
        _activeEnemies.Clear();
    }
}