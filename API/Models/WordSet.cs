using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace API.Models;

/// <summary>
/// Represents a set of word pairs in a specific language associated with a user.
/// </summary>
public class WordSet
{
    public int Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// The language code of the words in this set.
    /// </summary>
    public string LanguageCode { get; set; }

    /// <summary>
    /// The collection of word pairs in this set.
    /// </summary>
    public ICollection<WordPair> WordPairs { get; set; } = new List<WordPair>();

    /// <summary>
    /// The ID of the user who owns this word set.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Navigation property to the user who owns this word set.
    /// </summary>
    [JsonIgnore, XmlIgnore]
    public User User { get; set; }
}