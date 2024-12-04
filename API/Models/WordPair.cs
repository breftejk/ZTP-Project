namespace API.Models;

/// <summary>
/// Represents a pair of words with a source word and its translation.
/// </summary>
public class WordPair
{
    public int Id { get; set; }
    public string Word { get; set; }
    public string Translation { get; set; }

    /// <summary>
    /// Gets or sets the code of the associated language.
    /// </summary>
    public string LanguageCode { get; set; }
}