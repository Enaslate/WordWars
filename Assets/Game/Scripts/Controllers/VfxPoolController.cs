using UnityEngine;
using UnityEngine.Pool;

public class VfxPoolController : MonoBehaviour
{
    [SerializeField] protected Transform _container;
    [SerializeField] protected VfxView _prefab;
    [SerializeField] protected int _defaultCapacity = 10;
    [SerializeField] protected int _maxSize = 100;

    protected ObjectPool<VfxView> _pool;

    private void Awake()
    {
        if (_prefab == null)
        {
            Debug.LogError($"{_prefab.GetType().Name} prefab is missing!", this);
            return;
        }

        _pool = new ObjectPool<VfxView>(
            createFunc: () => Instantiate(_prefab, _container),
            actionOnGet: view =>
            {
                view.Setup(this);
                view.gameObject.SetActive(true);
                view.ParticleSystem.Play();
            },
            actionOnRelease: view =>
            {
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

    public VfxView Get()
    {
        if (_pool == null) return null;

        var view = _pool.Get();
        return view;
    }

    public void Release(VfxView view)
    {
        if (_pool == null) return;

        _pool.Release(view);
    }
}