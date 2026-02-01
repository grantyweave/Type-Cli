namespace TypeCLI.Core;

public enum InputAction
{
    None,
    Character,
    Backspace,
    Escape,
    Enter
}

public readonly struct InputResult
{
    public InputAction Action { get; }
    public char Character { get; }

    private InputResult(InputAction action, char character = '\0')
    {
        Action = action;
        Character = character;
    }

    public static InputResult None() => new(InputAction.None);
    public static InputResult Char(char c) => new(InputAction.Character, c);
    public static InputResult Backspace() => new(InputAction.Backspace);
    public static InputResult Escape() => new(InputAction.Escape);
    public static InputResult Enter() => new(InputAction.Enter);
}

public class InputHandler
{
    public InputResult TryReadInput()
    {
        if (!Console.KeyAvailable)
        {
            return InputResult.None();
        }

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        return keyInfo.Key switch
        {
            ConsoleKey.Backspace => InputResult.Backspace(),
            ConsoleKey.Escape => InputResult.Escape(),
            ConsoleKey.Enter => InputResult.Enter(),
            _ => IsTypableCharacter(keyInfo.KeyChar)
                ? InputResult.Char(keyInfo.KeyChar)
                : InputResult.None()
        };
    }

    private static bool IsTypableCharacter(char c)
    {
        return !char.IsControl(c) || c == ' ';
    }
}
