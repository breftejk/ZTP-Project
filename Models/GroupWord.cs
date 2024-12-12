namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents the relationship between a group and a word.
    /// </summary>
    public class GroupWord
    {
        /// <summary>
        /// Gets or sets the group ID associated with this relationship.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group associated with this relationship.
        /// </summary>
        public Group? Group { get; set; }

        /// <summary>
        /// Gets or sets the word ID associated with this relationship.
        /// </summary>
        public int WordId { get; set; }

        /// <summary>
        /// Gets or sets the word associated with this relationship.
        /// </summary>
        public Word? Word { get; set; }
    }
}