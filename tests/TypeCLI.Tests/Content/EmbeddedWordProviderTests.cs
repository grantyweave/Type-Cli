using TypeCLI.Content;

namespace TypeCLI.Tests.Content;

public class EmbeddedWordProviderTests
{
    private readonly EmbeddedWordProvider _provider;

    public EmbeddedWordProviderTests()
    {
        _provider = new EmbeddedWordProvider();
    }

    [Fact]
    public void GetRandomWords_ReturnsCorrectWordCount()
    {
        string result = _provider.GetRandomWords(10);

        int wordCount = result.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

        Assert.Equal(10, wordCount);
    }

    [Fact]
    public void GetRandomWords_WithZeroCount_ReturnsEmpty()
    {
        string result = _provider.GetRandomWords(0);

        Assert.Empty(result);
    }

    [Fact]
    public void GetRandomWords_ReturnsNonEmptyString()
    {
        string result = _provider.GetRandomWords(5);

        Assert.NotEmpty(result);
        Assert.DoesNotContain("  ", result); // No double spaces
    }

    [Fact]
    public void GetRandomQuote_ReturnsNonEmptyString()
    {
        string result = _provider.GetRandomQuote();

        Assert.NotEmpty(result);
    }

    [Fact]
    public void GetWordList_ReturnsNonEmptyList()
    {
        IReadOnlyList<string> words = _provider.GetWordList();

        Assert.NotEmpty(words);
        Assert.All(words, w => Assert.NotEmpty(w));
    }

    [Fact]
    public void GetQuoteList_ReturnsNonEmptyList()
    {
        IReadOnlyList<string> quotes = _provider.GetQuoteList();

        Assert.NotEmpty(quotes);
        Assert.All(quotes, q => Assert.NotEmpty(q));
    }

    [Fact]
    public void GetRandomWords_ProducesVariedResults()
    {
        string result1 = _provider.GetRandomWords(20);
        string result2 = _provider.GetRandomWords(20);

        // While theoretically could be the same, extremely unlikely with 20 words
        Assert.NotEqual(result1, result2);
    }
}
