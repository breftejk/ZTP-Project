namespace ZTP_Project.Models
{
    /// <summary>
    /// Represents an error view model for displaying error information.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indicates whether the request identifier should be displayed.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}