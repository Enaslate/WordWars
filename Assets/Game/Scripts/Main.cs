using UnityEngine;
using UnityEngine.InputSystem.XInput;

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
    private TextInputProcessor _inputProcessor;

    [Header("Score")]
    [SerializeField] private GameScoreView _gameScoreView;
    [SerializeField] private GameScore _gameScore;
    private GameScoreController _gameScoreController;

    private void Awake()
    {
        ConfigureInput();
        ConfigureGameScore();

        _enemyController = new EnemyController(_enemyPool, _sentencePool, _playerController);
        _battleController = new BattleController(_enemyController, _projectilePool, _inputProcessor, _gameScoreController);
        _gameplayController = new GameplayController(_inputController, _playerController, _enemyController, _battleController, _inputProcessor, _gameScoreController);
    }

    private void Start()
    {
        StartGameplay();
    }

    private void StartGameplay()
    {
        _gameplayController.Start();
        if (_playerController?.PlayerView?.Character == null)
        {
            Debug.LogError($"Player character is null!");
        }
        _playerController.PlayerView.Character.Died += OnGameOver;
    }

    private void ConfigureGameScore()
    {
        _gameScore = new GameScore();
        _gameScoreController = new GameScoreController(_gameScoreView, _gameScore);
        _gameScoreController.Hide();
    }

    private void ConfigureInput()
    {
        _inputActions = new InputActions();
        _inputController = new InputController(_inputActions);
        _inputProcessor = new TextInputProcessor();
    }

    public void RestartGameplay()
    {
        _gameScore.Reset();
        _gameScoreController.Hide();
        Time.timeScale = 1.0f;
        _playerController.Respawn();
        _inputController.Enable();
        StartGameplay();
    }

    private void Update()
    {
        var delta = Time.deltaTime;
        _gameScore.UpdateGameScore(delta);
    }

    private void OnGameOver(Character character)
    {
        if (character is not PlayerCharacter) return;

        _inputController.Disable();
        _enemyController.DespawnAll();
        _playerController.PlayerView.Character.Died -= OnGameOver;
        Time.timeScale = 0;
        _gameScoreController.Show();
        Debug.Log("Game over!");
    }

    private void OnDestroy()
    {
        _gameplayController.Unsubscribe();
        _inputController.Disable();
    }
}
