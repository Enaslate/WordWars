using System;

public class Enemy
{
    public event Action<Enemy> Died;

    public float MaxHealth { get; private set; } = 3f;
    public float Health { get; private set; }
    public int StartIndex { get; private set; }
    public string Sentence { get; private set; }

    public int LastMatchIndex = -1;

    public Enemy(int startIndex, string sentence)
    {
        StartIndex = startIndex;
        Health = MaxHealth;
        Sentence = sentence;
    }

    public void Damage(float damage)
    {
        Health = Math.Max(0, Health - damage);

        if (Health <= 0)
            Die();
    }

    public void MoveIndexes(int delta)
    {
        StartIndex = Math.Max(0, StartIndex - delta);
        LastMatchIndex = Math.Max(0, LastMatchIndex - delta);
    }

    private void Die()
    {
        Died?.Invoke(this);
    }
}