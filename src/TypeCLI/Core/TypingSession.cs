using TypeCLI.Content;
using TypeCLI.Models;

namespace TypeCLI.Core;

public class TypingSession
{
    private readonly IWordProvider _wordProvider;
    private GameState? _gameState;

    public GameState? CurrentState => _gameState;
    public bool IsActive => _gameState != null && !_gameState.IsComplete;

    public TypingSession(IWordProvider wordProvider)
    {
        _wordProvider = wordProvider;
    }

    public GameState StartNewSession(GameMode mode)
    {
        string targetText = GenerateTargetText(mode);
        _gameState = new GameState(mode, targetText);
        return _gameState;
    }

    public void ProcessInput(InputResult input)
    {
        if (_gameState == null || _gameState.IsComplete)
        {
            return;
        }

        switch (input.Action)
        {
            case InputAction.Character:
                _gameState.ProcessCharacter(input.Character);
                break;
            case InputAction.Backspace:
                _gameState.ProcessBackspace();
                break;
            case InputAction.Escape:
                _gameState.ForceComplete();
                break;
        }
    }

    public void Update()
    {
        _gameState?.CheckTimeExpired();
    }

    public SessionResult? GetResult()
    {
        if (_gameState == null)
        {
            return null;
        }

        bool wasCompleted = _gameState.CursorPosition >= _gameState.Characters.Count ||
                           (_gameState.TimeLimit.HasValue && _gameState.GetRemainingTime() <= TimeSpan.Zero);

        return SessionResult.Create(
            _gameState.Mode,
            _gameState.GetElapsedTime(),
            _gameState.Characters,
            wasCompleted);
    }

    public void EndSession()
    {
        _gameState?.ForceComplete();
    }

    private string GenerateTargetText(GameMode mode)
    {
        return mode switch
        {
            GameMode.Quote => _wordProvider.GetRandomQuote(),
            GameMode.Words10 => _wordProvider.GetRandomWords(10),
            GameMode.Words25 => _wordProvider.GetRandomWords(25),
            GameMode.Words50 => _wordProvider.GetRandomWords(50),
            GameMode.Words100 => _wordProvider.GetRandomWords(100),
            _ => _wordProvider.GetRandomWords(GetWordCountForTimedMode(mode))
        };
    }

    private static int GetWordCountForTimedMode(GameMode mode)
    {
        return mode switch
        {
            GameMode.Timed15 => 30,
            GameMode.Timed30 => 60,
            GameMode.Timed60 => 120,
            GameMode.Timed120 => 240,
            _ => 50
        };
    }
}
