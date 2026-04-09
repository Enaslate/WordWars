using UnityEngine;

public class BattleController
{
    private readonly EnemyController _enemyController;
    private readonly ProjectilePoolController _projectilePool;
    private readonly VfxPoolController _vfxPool;
    private readonly TextInputProcessor _inputProcessor;
    private readonly GameScoreController _gameScoreController;
    private readonly AudioController _audioController;

    public BattleController(EnemyController enemyController,
        ProjectilePoolController projectilePool,
        TextInputProcessor inputReader,
        GameScoreController gameScoreController,
        VfxPoolController vfxPool,
        AudioController audioController)
    {
        _enemyController = enemyController;
        _projectilePool = projectilePool;

        _inputProcessor = inputReader;
        _gameScoreController = gameScoreController;
        _vfxPool = vfxPool;
        _audioController = audioController;
    }

    public void Spawn()
    {
        var currentEnemyCountToSpawn = CalculateCurrentEnemyCountToSpawn();
        var speed = GetSpeedModifier();

        for (int i = _enemyController.Enemies.Count; i < currentEnemyCountToSpawn; i++)
        {
            var next = UnityEngine.Random.Range(0, SentenceStorageHelper.Sentences.Length - 1);
            var enemy = new Enemy(_inputProcessor.InputLength, SentenceStorageHelper.Sentences[next], speed);
            _enemyController.Spawn(enemy);
            enemy.Died += OnDied;
        }
    }

    public void Shoot(Vector3 startPosition, EnemyView target)
    {
        var projectileView = _projectilePool.Get();
        projectileView.Setup(startPosition, target, 100, _projectilePool);

        if (projectileView == null) return;
    }

    public void OnDied(Character character)
    {
        character.Died -= OnDied;

        if (character is not Enemy enemy) return;

        var explosion = _vfxPool.Get();
        _enemyController.Enemies.TryGetValue(enemy, out var enemyView);

        if (enemyView == null) return;

        var position = enemyView.transform.position;
        explosion.transform.position = position;
        _audioController.PlayExplosion(position);

        if (!enemy.IsSuicideAttack)
            _gameScoreController.Score.OnEnemyKilled(10);

        _enemyController.Despawn(enemy);
        Spawn();
    }

    public int CalculateCurrentEnemyCountToSpawn()
    {
        var timeBonus = _gameScoreController.Score.GameTimeSeconds / 60f;
        var additional = Mathf.FloorToInt(_gameScoreController.Score.Score / 500f + timeBonus);
        var maxEnemies = 8;
        return Mathf.Clamp(1 + additional, 1, maxEnemies);
    }

    public float GetSpeedModifier()
    {
        var score = _gameScoreController.Score;

        var baseSpeed = 1f;

        var timeSpeedModifier = score.GameTimeSeconds / 300f;
        var scoreSpeedModifier = score.Score / 5000f;
        var speed = baseSpeed + timeSpeedModifier + scoreSpeedModifier;

        return Mathf.Min(speed, 5f);
    }
}