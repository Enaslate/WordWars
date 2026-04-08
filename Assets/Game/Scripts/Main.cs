using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemyPoolController _enemyPool;
    [SerializeField] private ProjectilePoolController _projectilePool;
    [SerializeField] private SentencePoolController _sentencePool;
    [SerializeField] private VfxPoolController _vfxPool;
    [SerializeField] private SfxPoolController _sfxPool;
    [SerializeField] private AudioController _audioController;
    [SerializeField] private HudView _hudView;

    [Header("Hud")]
    [SerializeField] private Transform _score;
    [SerializeField] private Transform _health;
    [SerializeField] private Transform _mainMenu;

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
        _battleController = new BattleController(_enemyController, _projectilePool, _inputProcessor, _gameScoreController, _vfxPool, _audioController);
        _gameplayController = new GameplayController(_inputController, _playerController, _enemyController, _battleController, _inputProcessor, _gameScoreController, _audioController);
    }

    private void Start()
    {
        ToMainMenu();
    }

    public void StartGameplay()
    {
        _mainMenu.gameObject.SetActive(false);
        _score.gameObject.SetActive(true);
        _health.gameObject.SetActive(true);

        _gameplayController.Start();
        var character = _playerController?.PlayerView?.Character;
        if (character == null)
        {
            Debug.LogError($"Player character is null!");
        }
        _hudView.Setup(character);
        character.Died += OnGameOver;
    }

    public void ToMainMenu()
    {
        _gameScore.Reset();
        _gameScoreController.Hide();
        _mainMenu.gameObject.SetActive(true);
        _score.gameObject.SetActive(false);
        _health.gameObject.SetActive(false);
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

        GameOver();
    }

    private void GameOver()
    {
        _audioController.PlayExplosion(_playerController.transform.position);
        _inputController.Disable();
        _enemyController.DespawnAll();
        _playerController.PlayerView.Character.Died -= OnGameOver;
        _gameScoreController.Show();
        Debug.Log("Game over!");
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _gameplayController.Unsubscribe();
        _inputController.Disable();
    }
}
