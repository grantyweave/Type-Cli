using Spectre.Console;
using Spectre.Console.Rendering;
using TypeCLI.Models;

namespace TypeCLI.UI;

public class SpectreRenderer
{
    private const int TargetFrameRate = 60;
    private static readonly TimeSpan s_frameDelay = TimeSpan.FromMilliseconds(1000.0 / TargetFrameRate);

    public async Task RunGameLoopAsync(
        GameState state,
        Action<GameState> processInput,
        CancellationToken cancellationToken = default)
    {
        Console.CursorVisible = false;

        try
        {
            await AnsiConsole.Live(BuildLayout(state))
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .StartAsync(async ctx =>
                {
                    while (!state.IsComplete && !cancellationToken.IsCancellationRequested)
                    {
                        processInput(state);
                        ctx.UpdateTarget(BuildLayout(state));
                        await Task.Delay(s_frameDelay, cancellationToken);
                    }
                });
        }
        finally
        {
            Console.CursorVisible = true;
        }
    }

    private static IRenderable BuildLayout(GameState state)
    {
        var layout = new Grid();
        layout.AddColumn();

        layout.AddRow(CreateHeader());
        layout.AddRow(new Rule().RuleStyle("blue"));
        layout.AddRow(CreateModeAndTimerRow(state));
        layout.AddRow(new Rule().RuleStyle("grey"));
        layout.AddRow(TypingDisplay.CreateTypingPanel(state));
        layout.AddRow(new Rule().RuleStyle("grey"));
        layout.AddRow(StatsPanel.CreateStatsTable(state));
        layout.AddRow(CreateFooter());

        return layout;
    }

    private static IRenderable CreateHeader()
    {
        return new Markup("[bold blue]TypeCLI[/] [grey]v1.0[/]").Centered();
    }

    private static IRenderable CreateModeAndTimerRow(GameState state)
    {
        var table = new Table
        {
            Border = TableBorder.None,
            ShowHeaders = false,
            Expand = true
        };

        table.AddColumn(new TableColumn(string.Empty));
        table.AddColumn(new TableColumn(string.Empty).RightAligned());

        string modeText = $"[blue]Mode:[/] {state.Mode.GetDisplayName()}";
        string timeText = GetTimeText(state);

        table.AddRow(modeText, timeText);

        return table;
    }

    private static string GetTimeText(GameState state)
    {
        if (!state.HasStarted)
        {
            if (state.TimeLimit.HasValue)
            {
                return $"[grey]Time:[/] {state.TimeLimit.Value:mm\\:ss}";
            }

            return "[grey]Time:[/] --:--";
        }

        if (state.TimeLimit.HasValue)
        {
            TimeSpan remaining = state.GetRemainingTime();
            string color = remaining.TotalSeconds <= 10 ? "red bold" : "white";
            return $"[grey]Time:[/] [{color}]{remaining:mm\\:ss}[/]";
        }

        TimeSpan elapsed = state.GetElapsedTime();
        return $"[grey]Time:[/] [white]{elapsed:mm\\:ss}[/]";
    }

    private static IRenderable CreateFooter()
    {
        return new Markup("[grey]Press [bold]ESC[/] to quit | [bold]BACKSPACE[/] to correct[/]").Centered();
    }

    public void ShowResults(SessionResult result)
    {
        ResultsDisplay.Render(result);
    }

    public GameMode ShowModeSelection()
    {
        AnsiConsole.Clear();

        var titleFiglet = new FigletText("TypeCLI")
            .Centered()
            .Color(Color.Blue);

        AnsiConsole.Write(titleFiglet);
        AnsiConsole.WriteLine();

        var timedModes = new List<(string Label, GameMode Mode)>
        {
            ("15 seconds", GameMode.Timed15),
            ("30 seconds", GameMode.Timed30),
            ("60 seconds", GameMode.Timed60),
            ("120 seconds", GameMode.Timed120)
        };

        var wordModes = new List<(string Label, GameMode Mode)>
        {
            ("10 words", GameMode.Words10),
            ("25 words", GameMode.Words25),
            ("50 words", GameMode.Words50),
            ("100 words", GameMode.Words100)
        };

        var allModes = new List<(string Label, GameMode Mode)>();
        allModes.AddRange(timedModes);
        allModes.AddRange(wordModes);
        allModes.Add(("Quote", GameMode.Quote));

        string selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[blue]Select a game mode:[/]")
                .PageSize(12)
                .AddChoiceGroup("[yellow]Timed Modes[/]", timedModes.Select(m => m.Label))
                .AddChoiceGroup("[yellow]Word Count Modes[/]", wordModes.Select(m => m.Label))
                .AddChoiceGroup("[yellow]Other[/]", new[] { "Quote" }));

        return allModes.First(m => m.Label == selected).Mode;
    }

    public PlayAgainChoice ShowPlayAgainPrompt(GameMode currentMode)
    {
        AnsiConsole.WriteLine();

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[blue]Play again?[/] [grey](Mode: {currentMode.GetDisplayName()})[/]")
                .AddChoices("Play Again", "Change Mode", "Quit"));

        return choice switch
        {
            "Play Again" => PlayAgainChoice.PlayAgain,
            "Change Mode" => PlayAgainChoice.ChangeMode,
            _ => PlayAgainChoice.Quit
        };
    }

    public void ShowWelcome()
    {
        AnsiConsole.Clear();

        var titleFiglet = new FigletText("TypeCLI")
            .Centered()
            .Color(Color.Blue);

        AnsiConsole.Write(titleFiglet);

        AnsiConsole.MarkupLine("[grey]A terminal typing practice game[/]".PadLeft(50));
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();

        var features = new Panel(
            new Markup(
                "[green]*[/] Real-time WPM and accuracy tracking\n" +
                "[green]*[/] Multiple game modes (timed and word count)\n" +
                "[green]*[/] Color-coded character feedback\n" +
                "[green]*[/] Quote mode for varied practice"))
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader("[blue]Features[/]"),
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(features);
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
        Console.ReadKey(true);
    }
}
