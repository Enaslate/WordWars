using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private EnemyPoolController _enemyPoolController;

    private InputActions _inputActions;
    private InputController _inputController;
    private GameplayController _gameplayController;
    private EnemyController _enemyController;

    private void Awake()
    {
        ConfigureInput();

        _enemyController = new EnemyController(_enemyPoolController);
        _gameplayController = new GameplayController(_inputController, _enemyController);
    }

    private void Start()
    {
        _gameplayController.Start();
    }

    private void ConfigureInput()
    {
        _inputActions = new InputActions();
        _inputController = new InputController(_inputActions);
    }
}
