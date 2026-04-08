using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolController<T> : MonoBehaviour where T : View
{
    [SerializeField] protected Transform _container;
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 100;
    protected ObjectPool<T> _pool;

    private void Awake()
    {
        if (_prefab == null)
        {
            Debug.LogError($"{_prefab.GetType().Name} prefab is missing!", this);
            return;
        }

        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab, _container),
            actionOnGet: view => view.gameObject.SetActive(true),
            actionOnRelease: view =>
            {
                view.Reset();
                view.gameObject.SetActive(false);
            },
            actionOnDestroy: (view) => Destroy(view.gameObject),
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void OnDestroy()
    {
        _pool?.Dispose();
    }

    public T Get()
    {
        if (_pool == null) return null;

        var view = _pool.Get();
        return view;
    }

    public void Release(T view)
    {
        if (_pool == null) return;

        _pool.Release(view);
    }
}