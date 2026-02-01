using TypeCLI.Models;

namespace TypeCLI.Tests.Models;

public class GameStateTests
{
    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        var state = new GameState(GameMode.Words10, "hello world");

        Assert.Equal(GameMode.Words10, state.Mode);
        Assert.Equal("hello world", state.TargetText);
        Assert.Equal(11, state.Characters.Count);
        Assert.Equal(0, state.CursorPosition);
        Assert.False(state.HasStarted);
        Assert.False(state.IsComplete);
    }

    [Fact]
    public void Constructor_WithTimedMode_SetsTimeLimit()
    {
        var state = new GameState(GameMode.Timed30, "test");

        Assert.Equal(TimeSpan.FromSeconds(30), state.TimeLimit);
    }

    [Fact]
    public void ProcessCharacter_AdvancesCursorAndStartsTimer()
    {
        var state = new GameState(GameMode.Words10, "abc");

        bool result = state.ProcessCharacter('a');

        Assert.True(result);
        Assert.Equal(1, state.CursorPosition);
        Assert.True(state.HasStarted);
        Assert.Equal(CharacterStatus.Correct, state.Characters[0].Status);
    }

    [Fact]
    public void ProcessCharacter_WithIncorrectChar_StillAdvances()
    {
        var state = new GameState(GameMode.Words10, "abc");

        state.ProcessCharacter('x');

        Assert.Equal(1, state.CursorPosition);
        Assert.Equal(CharacterStatus.Incorrect, state.Characters[0].Status);
    }

    [Fact]
    public void ProcessBackspace_MovesCursorBack()
    {
        var state = new GameState(GameMode.Words10, "abc");
        state.ProcessCharacter('a');
        state.ProcessCharacter('b');

        bool result = state.ProcessBackspace();

        Assert.True(result);
        Assert.Equal(1, state.CursorPosition);
        Assert.Equal(CharacterStatus.Pending, state.Characters[1].Status);
    }

    [Fact]
    public void ProcessBackspace_AtStart_ReturnsFalse()
    {
        var state = new GameState(GameMode.Words10, "abc");

        bool result = state.ProcessBackspace();

        Assert.False(result);
        Assert.Equal(0, state.CursorPosition);
    }

    [Fact]
    public void ProcessCharacter_AtEnd_CompletesGame()
    {
        var state = new GameState(GameMode.Words10, "ab");
        state.ProcessCharacter('a');

        state.ProcessCharacter('b');

        Assert.True(state.IsComplete);
    }

    [Fact]
    public void ForceComplete_SetsIsComplete()
    {
        var state = new GameState(GameMode.Words10, "abc");

        state.ForceComplete();

        Assert.True(state.IsComplete);
    }

    [Fact]
    public void GetCorrectCount_ReturnsOnlyCorrectAndCorrected()
    {
        var state = new GameState(GameMode.Words10, "abcd");
        state.ProcessCharacter('a'); // Correct
        state.ProcessCharacter('x'); // Incorrect
        state.ProcessCharacter('c'); // Correct

        int correctCount = state.GetCorrectCount();

        Assert.Equal(2, correctCount);
    }

    [Fact]
    public void GetTypedCount_ReturnsAllTyped()
    {
        var state = new GameState(GameMode.Words10, "abcd");
        state.ProcessCharacter('a');
        state.ProcessCharacter('b');
        state.ProcessCharacter('c');

        int typedCount = state.GetTypedCount();

        Assert.Equal(3, typedCount);
    }
}
