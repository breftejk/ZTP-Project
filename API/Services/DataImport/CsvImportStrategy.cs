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

            if (columns.Length == 4 && IsValidWordPair(columns))
            {
                var wordPair = new WordPair
                {
                    Id = int.Parse(columns[0]),
                    LanguageCode = columns[1],
                    Word = columns[2],
                    Translation = columns[3]
                };

                _wordFacade.AddWordPair(wordPair.Word, wordPair.Translation, wordPair.LanguageCode);
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
        return !string.IsNullOrEmpty(columns[2]) && !string.IsNullOrEmpty(columns[3]) && !string.IsNullOrEmpty(columns[1]);
    }
}