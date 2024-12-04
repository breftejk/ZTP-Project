namespace API.Models;

/// <summary>
/// Represents an application user.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user (e.g., GUID or string).
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The collection of word sets associated with this user.
    /// </summary>
    public ICollection<WordSet> WordSets { get; set; } = new List<WordSet>();
}