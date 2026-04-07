using UnityEngine;

public class EnemyView : View
{
    [field: SerializeField] public SentenceView SentenceView { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }
    [SerializeField] private PlayerView _target;

    public void Setup(Vector3 position, Enemy enemy, SentenceView sentenceView, PlayerView target)
    {
        transform.position = position;
        Enemy = enemy;
        SentenceView = sentenceView;
        SentenceView.Setup(transform, enemy.Sentence);
        _target = target;
    }

    public void UpdateHighlight(string buffer)
    {
        if (SentenceView == null) return;

        SentenceView.UpdateHighlight(buffer, Enemy.StartIndex);
    }

    private void Update()
    {
        if (_target == null) return;

        var direction = (_target.transform.position - transform.position);
        var step = Enemy.Speed * Time.deltaTime;

        if (direction.magnitude <= Enemy.AttackRadius)
        {
            _target.Character.TakeDamage(Enemy.Damage);
            Enemy.Die();
        }
        else
        {
            transform.position += direction.normalized * step;
        }
    }

    public override void Reset()
    {
        Enemy = null;
        SentenceView.Reset();
        SentenceView = null;
    }
}