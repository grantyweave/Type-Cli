using TypeCLI.Models;

namespace TypeCLI.Tests.Models;

public class GameModeTests
{
    [Theory]
    [InlineData(GameMode.Timed15, true)]
    [InlineData(GameMode.Timed30, true)]
    [InlineData(GameMode.Timed60, true)]
    [InlineData(GameMode.Timed120, true)]
    [InlineData(GameMode.Words10, false)]
    [InlineData(GameMode.Words25, false)]
    [InlineData(GameMode.Quote, false)]
    public void IsTimed_ReturnsCorrectValue(GameMode mode, bool expected)
    {
        Assert.Equal(expected, mode.IsTimed());
    }

    [Theory]
    [InlineData(GameMode.Words10, true)]
    [InlineData(GameMode.Words25, true)]
    [InlineData(GameMode.Words50, true)]
    [InlineData(GameMode.Words100, true)]
    [InlineData(GameMode.Timed15, false)]
    [InlineData(GameMode.Quote, false)]
    public void IsWordCount_ReturnsCorrectValue(GameMode mode, bool expected)
    {
        Assert.Equal(expected, mode.IsWordCount());
    }

    [Theory]
    [InlineData(GameMode.Timed15, 15)]
    [InlineData(GameMode.Timed30, 30)]
    [InlineData(GameMode.Timed60, 60)]
    [InlineData(GameMode.Timed120, 120)]
    [InlineData(GameMode.Words10, 0)]
    public void GetDurationSeconds_ReturnsCorrectValue(GameMode mode, int expected)
    {
        Assert.Equal(expected, mode.GetDurationSeconds());
    }

    [Theory]
    [InlineData(GameMode.Words10, 10)]
    [InlineData(GameMode.Words25, 25)]
    [InlineData(GameMode.Words50, 50)]
    [InlineData(GameMode.Words100, 100)]
    [InlineData(GameMode.Timed15, 0)]
    public void GetWordCount_ReturnsCorrectValue(GameMode mode, int expected)
    {
        Assert.Equal(expected, mode.GetWordCount());
    }

    [Theory]
    [InlineData(GameMode.Timed15, "15 seconds")]
    [InlineData(GameMode.Words25, "25 words")]
    [InlineData(GameMode.Quote, "Quote")]
    public void GetDisplayName_ReturnsReadableString(GameMode mode, string expected)
    {
        Assert.Equal(expected, mode.GetDisplayName());
    }
}
