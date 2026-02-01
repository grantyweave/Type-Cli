using TypeCLI.Models;
using TypeCLI.Statistics;

namespace TypeCLI.Tests.Statistics;

public class WpmCalculatorTests
{
    [Fact]
    public void CalculateGrossWpm_WithValidInput_ReturnsCorrectWpm()
    {
        // 250 characters in 1 minute = 50 WPM (250/5 = 50 words)
        int totalCharacters = 250;
        var elapsed = TimeSpan.FromMinutes(1);

        double result = WpmCalculator.CalculateGrossWpm(totalCharacters, elapsed);

        Assert.Equal(50, result);
    }

    [Fact]
    public void CalculateGrossWpm_WithZeroTime_ReturnsZero()
    {
        int totalCharacters = 100;
        var elapsed = TimeSpan.Zero;

        double result = WpmCalculator.CalculateGrossWpm(totalCharacters, elapsed);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateGrossWpm_With30Seconds_ReturnsCorrectWpm()
    {
        // 125 characters in 30 seconds = 50 WPM (125/5 = 25 words, 25 / 0.5 min = 50 WPM)
        int totalCharacters = 125;
        var elapsed = TimeSpan.FromSeconds(30);

        double result = WpmCalculator.CalculateGrossWpm(totalCharacters, elapsed);

        Assert.Equal(50, result);
    }

    [Fact]
    public void CalculateNetWpm_WithErrors_SubtractsErrorPenalty()
    {
        // 250 chars, 10 errors in 1 minute
        // Gross WPM = 50, Error penalty = 10/1 = 10
        // Net WPM = 50 - 10 = 40
        int totalCharacters = 250;
        int errors = 10;
        var elapsed = TimeSpan.FromMinutes(1);

        double result = WpmCalculator.CalculateNetWpm(totalCharacters, errors, elapsed);

        Assert.Equal(40, result);
    }

    [Fact]
    public void CalculateNetWpm_WithManyErrors_ReturnsZeroMinimum()
    {
        // More errors than WPM should result in 0
        int totalCharacters = 50;
        int errors = 100;
        var elapsed = TimeSpan.FromMinutes(1);

        double result = WpmCalculator.CalculateNetWpm(totalCharacters, errors, elapsed);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateGrossWpm_WithCharacterResults_CountsTypedOnly()
    {
        var results = new List<CharacterResult>
        {
            CreateTypedCharacter('a', 'a'),
            CreateTypedCharacter('b', 'b'),
            CreateTypedCharacter('c', 'c'),
            CreateTypedCharacter('d', 'd'),
            CreateTypedCharacter('e', 'e'),
            new CharacterResult('f'), // Not typed
            new CharacterResult('g')  // Not typed
        };

        var elapsed = TimeSpan.FromMinutes(1);

        double result = WpmCalculator.CalculateGrossWpm(results, elapsed);

        // 5 typed chars / 5 = 1 word, 1 word / 1 minute = 1 WPM
        Assert.Equal(1, result);
    }

    private static CharacterResult CreateTypedCharacter(char expected, char typed)
    {
        var result = new CharacterResult(expected);
        result.SetTyped(typed);
        return result;
    }
}
