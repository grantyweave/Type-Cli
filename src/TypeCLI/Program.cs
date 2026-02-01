using TypeCLI.Core;
using TypeCLI.Models;

namespace TypeCLI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var engine = new GameEngine();

        engine.ShowWelcome();

        GameMode currentMode = engine.ShowModeSelection();
        bool running = true;

        while (running)
        {
            using var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            SessionResult? result = await engine.RunAsync(currentMode, cts.Token);

            if (result != null)
            {
                engine.ShowResults(result);
            }

            PlayAgainChoice choice = engine.ShowPlayAgainPrompt(currentMode);

            switch (choice)
            {
                case PlayAgainChoice.PlayAgain:
                    break;
                case PlayAgainChoice.ChangeMode:
                    currentMode = engine.ShowModeSelection();
                    break;
                case PlayAgainChoice.Quit:
                    running = false;
                    break;
            }
        }

        Console.Clear();
        Console.WriteLine("Thanks for practicing with TypeCLI!");
    }
}
