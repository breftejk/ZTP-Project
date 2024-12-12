using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ZTP_Project.Attributes;

namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents a word with its original text and translation.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Gets or sets the unique identifier of the word.
        /// </summary>
        [CsvIgnore, XmlIgnore, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the original text of the word.
        /// </summary>
        [Required, StringLength(100)]
        public string Original { get; set; }

        /// <summary>
        /// Gets or sets the translation of the word.
        /// </summary>
        [Required, StringLength(100)]
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the language ID associated with the word.
        /// </summary>
        [Required]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the language associated with the word.
        /// </summary>
        [CsvIgnore, XmlIgnore, JsonIgnore]
        public Language? Language { get; set; }
    }
}