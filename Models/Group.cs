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
        /// Gets or sets the identifier of the user who owns the group.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated language.
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

    /// <summary>
    /// Represents the relationship between a group and a word.
    /// </summary>
    public class GroupWord
    {
        /// <summary>
        /// Gets or sets the identifier of the group.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the associated group.
        /// </summary>
        public Group? Group { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the word.
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Gets or sets the associated word.
        /// </summary>
        public Word? Word { get; set; }
    }
}