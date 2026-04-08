public class GameScoreController
{
    public GameScore Score { get; private set; }

    private readonly GameScoreView _view;

    public GameScoreController(GameScoreView view, GameScore gameScore)
    {
        _view = view;
        SetGameScore(gameScore);    
    }

    public void SetGameScore(GameScore gameScore)
    {
        Score = gameScore;

        _view.Setup(Score);
    }

    public void Show()
    {
        _view.Show();
    }

    public void Hide()
    {
        _view.Hide();
    }
}