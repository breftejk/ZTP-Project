namespace API.Models;

/// <summary>
/// Represents a pair of words with a source word and its translation.
/// </summary>
public class WordPair
{
    public int Id { get; set; }
    public string Word { get; set; }
    public string Translation { get; set; }
    public string Language { get; set; }

    public WordPair(string word, string translation, string language)
    {
        Word = word;
        Translation = translation;
        Language = language;
    }
    
    // Parameterless constructor (required for serialization)
    public WordPair()
    {
    }
}