using TypeCLI.Models;
using TypeCLI.Statistics;

namespace TypeCLI.Tests.Statistics;

public class AccuracyCalculatorTests
{
    [Fact]
    public void CalculateAccuracy_WithAllCorrect_Returns100()
    {
        int correctCharacters = 100;
        int totalAttempts = 100;

        double result = AccuracyCalculator.CalculateAccuracy(correctCharacters, totalAttempts);

        Assert.Equal(100.0, result);
    }

    [Fact]
    public void CalculateAccuracy_With90PercentCorrect_Returns90()
    {
        int correctCharacters = 90;
        int totalAttempts = 100;

        double result = AccuracyCalculator.CalculateAccuracy(correctCharacters, totalAttempts);

        Assert.Equal(90.0, result);
    }

    [Fact]
    public void CalculateAccuracy_WithZeroAttempts_Returns100()
    {
        int correctCharacters = 0;
        int totalAttempts = 0;

        double result = AccuracyCalculator.CalculateAccuracy(correctCharacters, totalAttempts);

        Assert.Equal(100.0, result);
    }

    [Fact]
    public void CalculateAccuracy_WithCharacterResults_CalculatesCorrectly()
    {
        var results = new List<CharacterResult>
        {
            CreateTypedCharacter('a', 'a'), // Correct
            CreateTypedCharacter('b', 'b'), // Correct
            CreateTypedCharacter('c', 'x'), // Incorrect
            CreateTypedCharacter('d', 'd'), // Correct
            CreateTypedCharacter('e', 'e')  // Correct
        };

        double result = AccuracyCalculator.CalculateAccuracy(results);

        // 4 correct out of 5 attempts = 80%
        Assert.Equal(80.0, result);
    }

    [Fact]
    public void CalculateRawAccuracy_WithCharacterResults_CalculatesCorrectly()
    {
        var results = new List<CharacterResult>
        {
            CreateTypedCharacter('a', 'a'), // Correct
            CreateTypedCharacter('b', 'b'), // Correct
            CreateTypedCharacter('c', 'x'), // Incorrect
            new CharacterResult('d'),       // Not typed
            new CharacterResult('e')        // Not typed
        };

        double result = AccuracyCalculator.CalculateRawAccuracy(results);

        // 2 correct out of 3 typed = 66.67%
        Assert.Equal(66.67, result, 2);
    }

    [Fact]
    public void CalculateRawAccuracy_WithNoTypedCharacters_Returns100()
    {
        var results = new List<CharacterResult>
        {
            new CharacterResult('a'),
            new CharacterResult('b'),
            new CharacterResult('c')
        };

        double result = AccuracyCalculator.CalculateRawAccuracy(results);

        Assert.Equal(100.0, result);
    }

    private static CharacterResult CreateTypedCharacter(char expected, char typed)
    {
        var result = new CharacterResult(expected);
        result.SetTyped(typed);
        return result;
    }
}
