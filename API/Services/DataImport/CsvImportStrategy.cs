using API.Models;
using API.Services.Word;

namespace API.Services.DataImport;

public class CsvImportStrategy : IDataImportStrategy
{
    private readonly WordFacade _wordFacade;

    public CsvImportStrategy(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    public List<WordPair> Import(string data)
    {
        if (!IsValidCsv(data))
        {
            throw new ArgumentException("Invalid CSV format.");
        }

        var wordPairs = new List<WordPair>();
        var lines = data.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split(',');

            if (columns.Length == 3 && IsValidWordPair(columns))
            {
                var wordPair = _wordFacade.AddWordPair(columns[0], columns[1], columns[2]);
                wordPairs.Add(wordPair);
            }
            else
            {
                throw new ArgumentException("Invalid CSV data format (missing or incorrect fields).");
            }
        }

        return wordPairs;
    }

    private bool IsValidCsv(string data)
    {
        return !string.IsNullOrWhiteSpace(data) && data.Contains(",") && data.Contains("\n");
    }

    private bool IsValidWordPair(string[] columns)
    {
        return !string.IsNullOrEmpty(columns[0]) && !string.IsNullOrEmpty(columns[1]) && !string.IsNullOrEmpty(columns[2]);
    }
}