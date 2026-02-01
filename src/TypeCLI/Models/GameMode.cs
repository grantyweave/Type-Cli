namespace TypeCLI.Models;

public enum GameMode
{
    Timed15,
    Timed30,
    Timed60,
    Timed120,
    Words10,
    Words25,
    Words50,
    Words100,
    Quote
}

public static class GameModeExtensions
{
    public static bool IsTimed(this GameMode mode) => mode switch
    {
        GameMode.Timed15 or GameMode.Timed30 or GameMode.Timed60 or GameMode.Timed120 => true,
        _ => false
    };

    public static bool IsWordCount(this GameMode mode) => mode switch
    {
        GameMode.Words10 or GameMode.Words25 or GameMode.Words50 or GameMode.Words100 => true,
        _ => false
    };

    public static int GetDurationSeconds(this GameMode mode) => mode switch
    {
        GameMode.Timed15 => 15,
        GameMode.Timed30 => 30,
        GameMode.Timed60 => 60,
        GameMode.Timed120 => 120,
        _ => 0
    };

    public static int GetWordCount(this GameMode mode) => mode switch
    {
        GameMode.Words10 => 10,
        GameMode.Words25 => 25,
        GameMode.Words50 => 50,
        GameMode.Words100 => 100,
        _ => 0
    };

    public static string GetDisplayName(this GameMode mode) => mode switch
    {
        GameMode.Timed15 => "15 seconds",
        GameMode.Timed30 => "30 seconds",
        GameMode.Timed60 => "60 seconds",
        GameMode.Timed120 => "120 seconds",
        GameMode.Words10 => "10 words",
        GameMode.Words25 => "25 words",
        GameMode.Words50 => "50 words",
        GameMode.Words100 => "100 words",
        GameMode.Quote => "Quote",
        _ => mode.ToString()
    };
}
