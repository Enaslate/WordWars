using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [field: SerializeField] public Enemy Enemy { get; private set; }

    public void Setup(Vector3 position, Enemy enemy)
    {
        transform.position = position;
        Enemy = enemy;
    }

    public void Reset()
    {
        Enemy = null;
    }
}