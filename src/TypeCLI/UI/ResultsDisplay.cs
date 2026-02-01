using Spectre.Console;
using TypeCLI.Models;

namespace TypeCLI.UI;

public static class ResultsDisplay
{
    public static void Render(SessionResult result)
    {
        AnsiConsole.Clear();

        var titleFiglet = new FigletText("Results")
            .Centered()
            .Color(Color.Blue);

        AnsiConsole.Write(titleFiglet);
        AnsiConsole.WriteLine();

        RenderMainStats(result);
        AnsiConsole.WriteLine();

        RenderDetailedStats(result);
        AnsiConsole.WriteLine();

        RenderSessionInfo(result);
        AnsiConsole.WriteLine();
    }

    private static void RenderMainStats(SessionResult result)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Expand = true
        };

        table.AddColumn(new TableColumn("[bold blue]WPM[/]").Centered());
        table.AddColumn(new TableColumn("[bold blue]Accuracy[/]").Centered());

        string wpmColor = GetWpmColor(result.NetWpm);
        string accColor = GetAccuracyColor(result.Accuracy);

        table.AddRow(
            $"[{wpmColor} bold]{result.NetWpm:F1}[/]",
            $"[{accColor} bold]{result.Accuracy:F1}%[/]"
        );

        AnsiConsole.Write(table);
    }

    private static void RenderDetailedStats(SessionResult result)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow(
            "[grey]Gross WPM:[/]",
            $"[white]{result.GrossWpm:F1}[/]"
        );

        grid.AddRow(
            "[grey]Net WPM:[/]",
            $"[white]{result.NetWpm:F1}[/]"
        );

        grid.AddRow(
            "[grey]Characters:[/]",
            $"[green]{result.CorrectCharacters}[/] / [red]{result.IncorrectCharacters}[/] / [white]{result.TotalCharacters}[/]"
        );

        grid.AddRow(
            "[grey]Words Completed:[/]",
            $"[white]{result.WordsCompleted}[/]"
        );

        grid.AddRow(
            "[grey]Total Keystrokes:[/]",
            $"[white]{result.TotalAttempts}[/]"
        );

        var panel = new Panel(grid)
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader("[blue]Detailed Statistics[/]"),
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(panel);
    }

    private static void RenderSessionInfo(SessionResult result)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow(
            "[grey]Mode:[/]",
            $"[white]{result.Mode.GetDisplayName()}[/]"
        );

        grid.AddRow(
            "[grey]Duration:[/]",
            $"[white]{result.Duration:mm\\:ss\\.ff}[/]"
        );

        grid.AddRow(
            "[grey]Completed:[/]",
            result.WasCompleted ? "[green]Yes[/]" : "[yellow]No (cancelled)[/]"
        );

        var panel = new Panel(grid)
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader("[blue]Session Info[/]"),
            Padding = new Padding(2, 1)
        };

        AnsiConsole.Write(panel);
    }

    private static string GetWpmColor(double wpm)
    {
        return wpm switch
        {
            >= 80 => "green",
            >= 60 => "olive",
            >= 40 => "yellow",
            >= 20 => "orange3",
            _ => "red"
        };
    }

    private static string GetAccuracyColor(double accuracy)
    {
        return accuracy switch
        {
            >= 98 => "green",
            >= 95 => "olive",
            >= 90 => "yellow",
            >= 80 => "orange3",
            _ => "red"
        };
    }
}
