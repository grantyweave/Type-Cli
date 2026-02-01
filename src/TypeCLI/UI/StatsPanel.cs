using System.Text;
using Spectre.Console;
using TypeCLI.Models;

namespace TypeCLI.UI;

public static class StatsPanel
{
    private static readonly TableColumn s_wpmColumn = new TableColumn("[blue]WPM[/]").Centered();
    private static readonly TableColumn s_accuracyColumn = new TableColumn("[blue]Accuracy[/]").Centered();
    private static readonly TableColumn s_progressColumn = new TableColumn("[blue]Progress[/]").Centered();
    private static readonly TableColumn s_timeColumn = new TableColumn("[blue]Time[/]").Centered();

    [ThreadStatic]
    private static StringBuilder? t_builder;

    public static Table CreateStatsTable(GameState state)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Expand = true
        };

        table.AddColumn(s_wpmColumn);
        table.AddColumn(s_accuracyColumn);
        table.AddColumn(s_progressColumn);
        table.AddColumn(s_timeColumn);

        StringBuilder sb = t_builder ??= new StringBuilder(64);

        sb.Clear();
        sb.Append("[bold]").Append((int)state.GetCurrentWpm()).Append("[/]");
        string wpmText = sb.ToString();

        sb.Clear();
        sb.Append("[bold]").AppendFormat("{0:F1}", state.GetCurrentAccuracy()).Append("%[/]");
        string accuracyText = sb.ToString();

        sb.Clear();
        sb.Append("[bold]").Append(state.CursorPosition).Append("[/]/").Append(state.TotalCharacters);
        string progressText = sb.ToString();

        string timeText = GetTimeDisplay(state, sb);

        table.AddRow(wpmText, accuracyText, progressText, timeText);

        return table;
    }

    private static string GetTimeDisplay(GameState state, StringBuilder sb)
    {
        sb.Clear();

        if (state.TimeLimit.HasValue)
        {
            TimeSpan remaining = state.GetRemainingTime();
            string color = remaining.TotalSeconds <= 10 ? "red" : "white";
            sb.Append('[').Append(color).Append(']');
            AppendTimeSpan(sb, remaining);
            sb.Append("[/]");
            return sb.ToString();
        }

        TimeSpan elapsed = state.GetElapsedTime();
        sb.Append("[white]");
        AppendTimeSpan(sb, elapsed);
        sb.Append("[/]");
        return sb.ToString();
    }

    private static void AppendTimeSpan(StringBuilder sb, TimeSpan time)
    {
        int minutes = (int)time.TotalMinutes;
        int seconds = time.Seconds;
        if (minutes < 10)
        {
            sb.Append('0');
        }
        sb.Append(minutes).Append(':');
        if (seconds < 10)
        {
            sb.Append('0');
        }
        sb.Append(seconds);
    }

    public static Panel CreateModePanel(GameState state)
    {
        string modeText = $"[bold blue]Mode:[/] {state.Mode.GetDisplayName()}";

        return new Panel(new Markup(modeText))
        {
            Border = BoxBorder.None,
            Padding = new Padding(0)
        };
    }
}
