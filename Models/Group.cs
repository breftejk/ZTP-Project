using System.ComponentModel.DataAnnotations;

namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents a group of words associated with a user and a language.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the unique identifier of the group.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user ID who owns the group.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the language ID associated with the group.
        /// </summary>
        [Required]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the associated language of the group.
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Gets or sets the collection of group-word relationships.
        /// </summary>
        public ICollection<GroupWord> GroupWords { get; set; } = new List<GroupWord>();
    }
}