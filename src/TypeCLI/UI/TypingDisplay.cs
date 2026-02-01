using System.Buffers;
using System.Text;
using Spectre.Console;
using TypeCLI.Models;

namespace TypeCLI.UI;

public static class TypingDisplay
{
    private const int MaxCharsPerLine = 60;

    private static readonly string[] s_statusPrefixes =
    {
        "[grey]",           // Pending = 0
        "[green]",          // Correct = 1
        "[red]",            // Incorrect = 2
        "[olive]"           // Corrected = 3
    };

    private const string CursorPrefix = "[black on yellow]";
    private const string TagSuffix = "[/]";

    [ThreadStatic]
    private static StringBuilder? t_builder;

    public static string BuildMarkup(GameState state)
    {
        StringBuilder builder = t_builder ??= new StringBuilder(1024);
        builder.Clear();

        int lineLength = 0;
        int cursorPos = state.CursorPosition;
        IReadOnlyList<CharacterResult> characters = state.Characters;
        int count = characters.Count;

        for (int i = 0; i < count; i++)
        {
            CharacterResult charResult = characters[i];
            char expected = charResult.Expected;

            if (expected == ' ' && lineLength > MaxCharsPerLine)
            {
                builder.Append('\n');
                lineLength = 0;
                continue;
            }

            bool isCursor = i == cursorPos;
            AppendCharacterMarkup(builder, charResult, expected, isCursor);
            lineLength++;
        }

        return builder.ToString();
    }

    private static void AppendCharacterMarkup(
        StringBuilder builder,
        CharacterResult charResult,
        char displayChar,
        bool isCursor)
    {
        string prefix = isCursor ? CursorPrefix : s_statusPrefixes[(int)charResult.Status];
        builder.Append(prefix);
        AppendEscapedChar(builder, displayChar);
        builder.Append(TagSuffix);
    }

    private static void AppendEscapedChar(StringBuilder builder, char c)
    {
        if (c == '[' || c == ']')
        {
            builder.Append('[').Append(c).Append(']');
        }
        else
        {
            builder.Append(c);
        }
    }

    public static Panel CreateTypingPanel(GameState state)
    {
        string markup = BuildMarkup(state);

        return new Panel(new Markup(markup))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1),
            Header = new PanelHeader("[blue]Type the text below[/]")
        };
    }
}
