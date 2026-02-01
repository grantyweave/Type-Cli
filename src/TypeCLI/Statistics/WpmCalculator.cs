using TypeCLI.Models;

namespace TypeCLI.Statistics;

public static class WpmCalculator
{
    private const double CharsPerWord = 5.0;

    public static double CalculateGrossWpm(int totalCharacters, TimeSpan elapsed)
    {
        if (elapsed.TotalMinutes <= 0)
        {
            return 0;
        }

        double words = totalCharacters / CharsPerWord;
        return words / elapsed.TotalMinutes;
    }

    public static double CalculateNetWpm(int totalCharacters, int errors, TimeSpan elapsed)
    {
        if (elapsed.TotalMinutes <= 0)
        {
            return 0;
        }

        double grossWpm = CalculateGrossWpm(totalCharacters, elapsed);
        double errorPenalty = errors / elapsed.TotalMinutes;

        return Math.Max(0, grossWpm - errorPenalty);
    }

    public static double CalculateGrossWpm(IReadOnlyList<CharacterResult> results, TimeSpan elapsed)
    {
        int typedCount = results.Count(r => r.IsTyped);
        return CalculateGrossWpm(typedCount, elapsed);
    }

    public static double CalculateNetWpm(IReadOnlyList<CharacterResult> results, TimeSpan elapsed)
    {
        int typedCount = results.Count(r => r.IsTyped);
        int errorCount = results.Count(r => r.Status == CharacterStatus.Incorrect);

        return CalculateNetWpm(typedCount, errorCount, elapsed);
    }
}
