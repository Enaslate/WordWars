using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemyPoolController _enemyPool;
    [SerializeField] private ProjectilePoolController _projectilePool;
    [SerializeField] private SentencePoolController _sentencePool;

    private InputActions _inputActions;
    private InputController _inputController;
    private GameplayController _gameplayController;
    private EnemyController _enemyController;
    private BattleController _battleController;
    private InputReader _inputReader;

    private void Awake()
    {
        ConfigureInput();

        _enemyController = new EnemyController(_enemyPool, _sentencePool, _playerController);
        _battleController = new BattleController(_enemyController, _projectilePool, _inputReader);
        _gameplayController = new GameplayController(_inputController, _playerController, _enemyController, _battleController, _inputReader);
    }

    private void Start()
    {
        _gameplayController.Start();
    }

    private void ConfigureInput()
    {
        _inputActions = new InputActions();
        _inputController = new InputController(_inputActions);
        _inputReader = new InputReader();
    }

    private void OnDestroy()
    {
        _gameplayController.Unsubscribe();
    }
}
