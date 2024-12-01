using System.Xml;
using System.Xml.Serialization;
using API.Models;
using API.Services.Word;

namespace API.Services.DataImport;

public class XmlImportStrategy : IDataImportStrategy
{
    private readonly WordFacade _wordFacade;

    public XmlImportStrategy(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    public List<WordPair> Import(string data)
    {
        if (!IsValidXml(data))
        {
            throw new ArgumentException("Invalid XML format.");
        }

        var serializer = new XmlSerializer(typeof(List<WordPair>));
        using var stringReader = new StringReader(data);
        var wordPairs = (List<WordPair>)serializer.Deserialize(stringReader);

        foreach (var wordPair in wordPairs)
        {
            if (IsValidWordPair(wordPair))
            {
                _wordFacade.AddWordPair(wordPair.Word, wordPair.Translation, wordPair.LanguageCode);
            }
            else
            {
                throw new ArgumentException("Invalid XML data format (missing or incorrect fields).");
            }
        }

        return wordPairs;
    }

    private bool IsValidXml(string data)
    {
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(data);
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