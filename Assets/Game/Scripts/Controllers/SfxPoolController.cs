using UnityEngine;
using UnityEngine.Pool;

public class SfxPoolController : MonoBehaviour
{
    [SerializeField] protected Transform _container;
    [SerializeField] protected SfxView _prefab;
    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 100;

    protected ObjectPool<SfxView> _pool;

    private void Awake()
    {
        if (_prefab == null)
        {
            Debug.LogError($"{_prefab.GetType().Name} prefab is missing!", this);
            return;
        }

        _pool = new ObjectPool<SfxView>(
            createFunc: () => Instantiate(_prefab, _container),
            actionOnGet: view =>
            {
                view.Setup(this);
                view.gameObject.SetActive(true);
            },
            actionOnRelease: view =>
            {
                view.gameObject.SetActive(false);
                view.Reset();
            },
            actionOnDestroy: (view) => Destroy(view.gameObject),
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize);
    }

    private void OnDestroy()
    {
        _pool?.Dispose();
    }

    public SfxView Get()
    {
        if (_pool == null) return null;

        var view = _pool.Get();
        return view;
    }

    public void Release(SfxView view)
    {
        if (_pool == null) return;

        _pool.Release(view);
    }
}