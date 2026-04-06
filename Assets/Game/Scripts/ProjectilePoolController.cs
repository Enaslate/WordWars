using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolController : MonoBehaviour
{
    [SerializeField] private ProjectileView _projectilePrefab;
    private ObjectPool<ProjectileView> _enemyPool;

    private void Awake()
    {
        if (_projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is missing!", this);
            return;
        }

        _enemyPool = new ObjectPool<ProjectileView>(
            createFunc: () => Instantiate(_projectilePrefab, transform),
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
    }

    public ProjectileView Get()
    {
        if (_enemyPool == null) return null;

        var view = _enemyPool.Get();
        return view;
    }

    public void Release(ProjectileView view)
    {
        if (_enemyPool == null) return;

        _enemyPool.Release(view);
    }
}