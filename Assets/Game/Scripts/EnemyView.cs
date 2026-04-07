using UnityEngine;

public class EnemyView : View
{
    [field: SerializeField] public SentenceView SentenceView { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }

    public void Setup(Vector3 position, Enemy enemy, SentenceView sentenceView)
    {
        transform.position = position;
        Enemy = enemy;
        SentenceView = sentenceView;
        SentenceView.Setup(transform, enemy.Sentence);
    }

    public void UpdateHighlight(string buffer)
    {
        SentenceView.UpdateHighlight(buffer, Enemy.StartIndex);
    }

    public override void Reset()
    {
        Enemy = null;
        SentenceView.Reset();
        SentenceView = null;
    }
}