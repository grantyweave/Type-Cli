using System.Reflection;
using System.Text.Json;

namespace TypeCLI.Content;

public class EmbeddedWordProvider : IWordProvider
{
    private readonly List<string> _words;
    private readonly List<string> _quotes;
    private readonly Random _random;

    public EmbeddedWordProvider()
    {
        _random = new Random();
        _words = LoadWords();
        _quotes = LoadQuotes();
    }

    public string GetRandomWords(int count)
    {
        if (_words.Count == 0)
        {
            return string.Empty;
        }

        var selectedWords = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            int index = _random.Next(_words.Count);
            selectedWords.Add(_words[index]);
        }

        return string.Join(" ", selectedWords);
    }

    public string GetRandomQuote()
    {
        if (_quotes.Count == 0)
        {
            return "The quick brown fox jumps over the lazy dog.";
        }

        int index = _random.Next(_quotes.Count);
        return _quotes[index];
    }

    public IReadOnlyList<string> GetWordList() => _words.AsReadOnly();

    public IReadOnlyList<string> GetQuoteList() => _quotes.AsReadOnly();

    private static List<string> LoadWords()
    {
        string json = LoadEmbeddedResource("TypeCLI.Data.words_common.json");
        if (string.IsNullOrEmpty(json))
        {
            return GetFallbackWords();
        }

        try
        {
            var document = JsonDocument.Parse(json);
            var words = new List<string>();

            if (document.RootElement.TryGetProperty("words", out JsonElement wordsElement))
            {
                foreach (JsonElement word in wordsElement.EnumerateArray())
                {
                    string? value = word.GetString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        words.Add(value);
                    }
                }
            }

            return words.Count > 0 ? words : GetFallbackWords();
        }
        catch
        {
            return GetFallbackWords();
        }
    }

    private static List<string> LoadQuotes()
    {
        string json = LoadEmbeddedResource("TypeCLI.Data.quotes.json");
        if (string.IsNullOrEmpty(json))
        {
            return GetFallbackQuotes();
        }

        try
        {
            var document = JsonDocument.Parse(json);
            var quotes = new List<string>();

            if (document.RootElement.TryGetProperty("quotes", out JsonElement quotesElement))
            {
                foreach (JsonElement quote in quotesElement.EnumerateArray())
                {
                    string? value = quote.GetString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        quotes.Add(value);
                    }
                }
            }

            return quotes.Count > 0 ? quotes : GetFallbackQuotes();
        }
        catch
        {
            return GetFallbackQuotes();
        }
    }

    private static string LoadEmbeddedResource(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private static List<string> GetFallbackWords()
    {
        return new List<string>
        {
            "the", "be", "to", "of", "and", "a", "in", "that", "have", "I",
            "it", "for", "not", "on", "with", "he", "as", "you", "do", "at",
            "this", "but", "his", "by", "from", "they", "we", "say", "her", "she",
            "or", "an", "will", "my", "one", "all", "would", "there", "their", "what",
            "so", "up", "out", "if", "about", "who", "get", "which", "go", "me"
        };
    }

    private static List<string> GetFallbackQuotes()
    {
        return new List<string>
        {
            "The quick brown fox jumps over the lazy dog.",
            "The only way to do great work is to love what you do.",
            "In the middle of difficulty lies opportunity."
        };
    }
}
