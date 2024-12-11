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
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the original text of the word.
        /// </summary>
        [Required(ErrorMessage = "Original word is required.")]
        [StringLength(100, ErrorMessage = "Original word must be at most 100 characters.")]
        public string Original { get; set; }

        /// <summary>
        /// Gets or sets the translation of the word.
        /// </summary>
        [Required(ErrorMessage = "Translation is required.")]
        [StringLength(100, ErrorMessage = "Translation must be at most 100 characters.")]
        public string Translation { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated language.
        /// </summary>
        [Required(ErrorMessage = "Language is required.")]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the associated language of the word.
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        [CsvIgnore]
        public Language? Language { get; set; }
    }
}