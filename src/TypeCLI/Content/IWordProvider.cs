namespace TypeCLI.Content;

public interface IWordProvider
{
    string GetRandomWords(int count);
    string GetRandomQuote();
    IReadOnlyList<string> GetWordList();
    IReadOnlyList<string> GetQuoteList();
}
