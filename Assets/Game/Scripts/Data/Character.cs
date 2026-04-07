using System;
using UnityEngine;

public abstract class Character
{
    public event Action<Character> Died;

    [field: SerializeField] public float MaxHealth { get; private set; } = 3f;
    [field: SerializeField] public float Health { get; private set; }

    protected Character()
    {
        Health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        Health = Math.Max(0, Health - damage);

        if (Health <= 0)
            Die();
    }

    public void Die()
    {
        Died?.Invoke(this);
    }
}