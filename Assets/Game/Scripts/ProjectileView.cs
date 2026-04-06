using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _stopDistance = 0.1f;

    private Transform _targetTransform;
    private Enemy _targetEnemy;
    private ProjectilePoolController _pool;
    private float _damage;

    public void Setup(Vector3 position, EnemyView targetEnemy, float damage, ProjectilePoolController pool)
    {
        transform.position = position;
        _targetEnemy = targetEnemy.Enemy;
        _targetTransform = targetEnemy.transform;
        _damage = damage;
        _pool = pool;
    }

    private void Update()
    {
        if (_targetTransform == null)
        {
            ReturnToPool();
            return;
        }

        Vector3 direction = _targetTransform.position - transform.position;
        float step = _speed * Time.deltaTime;

        if (direction.magnitude <= _stopDistance)
        {
            _targetEnemy?.Damage(_damage);
            ReturnToPool();
        }
        else
        {
            transform.position += direction.normalized * step;
        }
    }

    private void ReturnToPool()
    {
        _pool?.Release(this);
    }

    public void Reset()
    {
        _targetTransform = null;
        _targetEnemy = null;
        _pool = null;
        _damage = 0;
    }
}