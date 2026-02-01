namespace TypeCLI.Models;

public class SessionResult
{
    public GameMode Mode { get; init; }
    public TimeSpan Duration { get; init; }
    public int TotalCharacters { get; init; }
    public int CorrectCharacters { get; init; }
    public int IncorrectCharacters { get; init; }
    public int TotalAttempts { get; init; }
    public double GrossWpm { get; init; }
    public double NetWpm { get; init; }
    public double Accuracy { get; init; }
    public int WordsCompleted { get; init; }
    public bool WasCompleted { get; init; }
    public DateTime CompletedAt { get; init; }

    public static SessionResult Create(
        GameMode mode,
        TimeSpan duration,
        IReadOnlyList<CharacterResult> results,
        bool wasCompleted)
    {
        int totalCharacters = results.Count(r => r.IsTyped);
        int correctCharacters = results.Count(r => r.Status == CharacterStatus.Correct);
        int incorrectCharacters = results.Count(r => r.Status == CharacterStatus.Incorrect);
        int correctedCharacters = results.Count(r => r.Status == CharacterStatus.Corrected);
        int totalAttempts = results.Sum(r => r.Attempts);

        double minutes = duration.TotalMinutes;
        double grossWpm = minutes > 0 ? (totalCharacters / 5.0) / minutes : 0;
        double errors = incorrectCharacters;
        double netWpm = minutes > 0 ? Math.Max(0, grossWpm - (errors / minutes)) : 0;
        double accuracy = totalAttempts > 0 ? ((double)(correctCharacters + correctedCharacters) / totalAttempts) * 100 : 0;

        int wordsCompleted = CountWordsCompleted(results);

        return new SessionResult
        {
            Mode = mode,
            Duration = duration,
            TotalCharacters = totalCharacters,
            CorrectCharacters = correctCharacters + correctedCharacters,
            IncorrectCharacters = incorrectCharacters,
            TotalAttempts = totalAttempts,
            GrossWpm = Math.Round(grossWpm, 1),
            NetWpm = Math.Round(netWpm, 1),
            Accuracy = Math.Round(accuracy, 1),
            WordsCompleted = wordsCompleted,
            WasCompleted = wasCompleted,
            CompletedAt = DateTime.Now
        };
    }

    private static int CountWordsCompleted(IReadOnlyList<CharacterResult> results)
    {
        int words = 0;
        bool inWord = false;

        foreach (CharacterResult result in results)
        {
            if (!result.IsTyped)
            {
                break;
            }

            if (result.Expected == ' ')
            {
                if (inWord)
                {
                    words++;
                    inWord = false;
                }
            }
            else
            {
                inWord = true;
            }
        }

        if (inWord)
        {
            words++;
        }

        return words;
    }
}
