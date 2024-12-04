using System.Text.Json;
using API.Models;
using API.Services;

namespace API.Strategies.DataImport;

public class JsonImportStrategy : IDataImportStrategy
{
    private readonly IWordService _wordService;

    public JsonImportStrategy(IWordService wordService)
    {
        _wordService = wordService;
    }

    public List<WordPair> Import(string data)
    {
        if (!IsValidJson(data))
        {
            throw new ArgumentException("Invalid JSON format.");
        }

        var wordPairs = JsonSerializer.Deserialize<List<WordPair>>(data);

        foreach (var wordPair in wordPairs)
        {
            if (IsValidWordPair(wordPair))
            {
                wordPair.Id = _wordService.AddWordPair(wordPair.Word, wordPair.Translation, wordPair.LanguageCode)
                    .Id;
            }
            else
            {
                throw new ArgumentException("Invalid JSON data format (missing or incorrect fields).");
            }
        }

        return wordPairs;
    }

    private bool IsValidJson(string data)
    {
        try
        {
            JsonSerializer.Deserialize<object>(data);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidWordPair(WordPair wordPair)
    {
        return !string.IsNullOrEmpty(wordPair.Word) && !string.IsNullOrEmpty(wordPair.Translation) && !string.IsNullOrEmpty(wordPair.LanguageCode);
    }
}