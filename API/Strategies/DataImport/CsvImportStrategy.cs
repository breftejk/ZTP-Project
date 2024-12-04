using API.Models;
using API.Services;

namespace API.Strategies.DataImport;

public class CsvImportStrategy : IDataImportStrategy
{
    private readonly IWordService _wordService;

    public CsvImportStrategy(IWordService wordService)
    {
        _wordService = wordService;
    }

    public List<WordPair> Import(string data)
    {
        if (!IsValidCsv(data))
        {
            throw new ArgumentException("Invalid CSV format.");
        }

        var wordPairs = new List<WordPair>();
        var lines = data.Split('\n');
        
        if(!AllWordPairsValid(wordPairs)) throw new ArgumentException("Invalid CSV data format (missing or incorrect fields).");

        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split(',');

            var wordPair = _wordService.AddWordPair(columns[0], columns[1], columns[2]);
            wordPairs.Add(wordPair);
        }

        return wordPairs;
    }

    private bool IsValidCsv(string data)
    {
        return !string.IsNullOrWhiteSpace(data) && data.Contains(",") && data.Contains("\n");
    }

    private bool IsValidWordPair(string[] columns)
    {
        if (string.IsNullOrEmpty(columns[0]) && string.IsNullOrEmpty(columns[1]) &&
            string.IsNullOrEmpty(columns[2])) return false;
        
        return true;
    }
    
    private bool AllWordPairsValid(List<WordPair> wordPairs)
    {
        return wordPairs.All(wordPair => IsValidWordPair(new[] {wordPair.Word, wordPair.Translation, wordPair.LanguageCode}));
    }
}