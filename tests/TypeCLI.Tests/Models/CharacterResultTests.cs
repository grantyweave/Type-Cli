using TypeCLI.Models;

namespace TypeCLI.Tests.Models;

public class CharacterResultTests
{
    [Fact]
    public void NewCharacterResult_HasPendingStatus()
    {
        var result = new CharacterResult('a');

        Assert.Equal(CharacterStatus.Pending, result.Status);
        Assert.Equal('a', result.Expected);
        Assert.Null(result.Typed);
        Assert.Equal(0, result.Attempts);
        Assert.False(result.IsCorrect);
        Assert.False(result.IsTyped);
    }

    [Fact]
    public void SetTyped_WithCorrectCharacter_SetsCorrectStatus()
    {
        var result = new CharacterResult('a');

        result.SetTyped('a');

        Assert.Equal(CharacterStatus.Correct, result.Status);
        Assert.Equal('a', result.Typed);
        Assert.Equal(1, result.Attempts);
        Assert.True(result.IsCorrect);
        Assert.True(result.IsTyped);
    }

    [Fact]
    public void SetTyped_WithIncorrectCharacter_SetsIncorrectStatus()
    {
        var result = new CharacterResult('a');

        result.SetTyped('b');

        Assert.Equal(CharacterStatus.Incorrect, result.Status);
        Assert.Equal('b', result.Typed);
        Assert.Equal(1, result.Attempts);
        Assert.False(result.IsCorrect);
        Assert.True(result.IsTyped);
    }

    [Fact]
    public void SetTyped_AfterCorrection_SetsCorrectedStatus()
    {
        var result = new CharacterResult('a');

        result.SetTyped('b'); // First attempt - incorrect
        result.Reset();
        result.SetTyped('a'); // Second attempt - correct

        Assert.Equal(CharacterStatus.Corrected, result.Status);
        Assert.Equal(2, result.Attempts);
        Assert.True(result.IsCorrect);
    }

    [Fact]
    public void Reset_ClearsTypedAndSetsStatusToPending()
    {
        var result = new CharacterResult('a');
        result.SetTyped('b');

        result.Reset();

        Assert.Equal(CharacterStatus.Pending, result.Status);
        Assert.Null(result.Typed);
        Assert.False(result.IsTyped);
        // Attempts should remain to track corrections
        Assert.Equal(1, result.Attempts);
    }
}
