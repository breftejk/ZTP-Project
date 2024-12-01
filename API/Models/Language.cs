using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// Represents a language used in word pairs.
/// </summary>
public class Language
{
    /// <summary>
    /// Gets or sets the ISO 639-1 code for the language (e.g., "en", "de").
    /// This serves as the primary key.
    /// </summary>
    [Key]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the name of the language (e.g., English, German).
    /// </summary>
    public string Name { get; set; }
}