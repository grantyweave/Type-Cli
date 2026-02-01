using TypeCLI.Models;

namespace TypeCLI.Statistics;

public static class AccuracyCalculator
{
    public static double CalculateAccuracy(int correctCharacters, int totalAttempts)
    {
        if (totalAttempts <= 0)
        {
            return 100.0;
        }

        return (correctCharacters / (double)totalAttempts) * 100.0;
    }

    public static double CalculateAccuracy(IReadOnlyList<CharacterResult> results)
    {
        int totalAttempts = results.Sum(r => r.Attempts);
        int correctCharacters = results.Count(r => r.IsCorrect);

        return CalculateAccuracy(correctCharacters, totalAttempts);
    }

    public static double CalculateRawAccuracy(IReadOnlyList<CharacterResult> results)
    {
        int typedCount = results.Count(r => r.IsTyped);
        if (typedCount == 0)
        {
            return 100.0;
        }

        int correctCount = results.Count(r => r.IsCorrect);
        return (correctCount / (double)typedCount) * 100.0;
    }
}
