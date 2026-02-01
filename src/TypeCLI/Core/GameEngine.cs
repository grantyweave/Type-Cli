using TypeCLI.Content;
using TypeCLI.Models;
using TypeCLI.UI;

namespace TypeCLI.Core;

public class GameEngine
{
    private readonly IWordProvider _wordProvider;
    private readonly InputHandler _inputHandler;
    private readonly SpectreRenderer _renderer;
    private readonly TypingSession _session;

    public GameEngine(IWordProvider? wordProvider = null)
    {
        _wordProvider = wordProvider ?? new EmbeddedWordProvider();
        _inputHandler = new InputHandler();
        _renderer = new SpectreRenderer();
        _session = new TypingSession(_wordProvider);
    }

    public async Task<SessionResult?> RunAsync(GameMode mode, CancellationToken cancellationToken = default)
    {
        GameState gameState = _session.StartNewSession(mode);

        try
        {
            await _renderer.RunGameLoopAsync(gameState, ProcessInputs, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _session.EndSession();
        }

        return _session.GetResult();
    }

    private void ProcessInputs(GameState gameState)
    {
        InputResult input = _inputHandler.TryReadInput();

        if (input.Action != InputAction.None)
        {
            _session.ProcessInput(input);
        }

        _session.Update();
    }

    public void ShowResults(SessionResult result)
    {
        _renderer.ShowResults(result);
    }

    public GameMode ShowModeSelection()
    {
        return _renderer.ShowModeSelection();
    }

    public PlayAgainChoice ShowPlayAgainPrompt(GameMode currentMode)
    {
        return _renderer.ShowPlayAgainPrompt(currentMode);
    }

    public void ShowWelcome()
    {
        _renderer.ShowWelcome();
    }
}
