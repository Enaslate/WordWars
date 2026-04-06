using System;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] public Enemy _enemy;

    public void Setup(Vector3 position, Enemy enemy)
    {
        transform.position = position;
        _enemy = enemy;
    }

    public void Reset()
    {
        _enemy = null;
    }
}