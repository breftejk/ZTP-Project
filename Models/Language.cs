using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ZTP_Project.Attributes;

namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents a language used in the application.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or sets the unique identifier of the language.
        /// </summary>
        [CsvIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code of the language (e.g., "EN", "PL").
        /// </summary>
        [Required]
        [StringLength(5, ErrorMessage = "Code must be at most 5 characters long.")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the language.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters long.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of groups associated with the language.
        /// </summary>
        [CsvIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public ICollection<Group>? Groups { get; set; } = new List<Group>();

        /// <summary>
        /// Gets or sets the collection of words associated with the language.
        /// </summary>
        [CsvIgnore]
        [XmlIgnore]
        [JsonIgnore]
        public ICollection<Word>? Words { get; set; } = new List<Word>();
    }
}