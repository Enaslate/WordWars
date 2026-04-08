using System;
using UnityEngine;

[Serializable]
public class Enemy : Character
{
    [field: SerializeField] public string Sentence { get; private set; }
    [field: SerializeField] public float Speed { get; private set; } = 1f;
    [field: SerializeField] public float AttackRadius { get; private set; } = 1f;
    [field: SerializeField] public float Damage { get; private set; } = 1f;

    public bool IsSuicideAttack;

    public int StartIndex { get; private set; }
    public int LastMatchIndex = -1;

    public Enemy(int startIndex, string sentence)
    {
        StartIndex = startIndex;
        Sentence = sentence;
    }

    public void MoveIndexes(int delta)
    {
        StartIndex = Math.Max(0, StartIndex - delta);
        LastMatchIndex = Math.Max(0, LastMatchIndex - delta);
    }
}