using ZTP_Project.Data;
using System.ComponentModel.DataAnnotations;

namespace ZTP_Project.Attributes
{
    /// <summary>
    /// Attribute for validating the uniqueness of a property value in the database.
    /// </summary>
    public class UniqueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Checks whether the value of the property is unique in the database.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">Context for the validation, providing access to the service provider.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating success if the value is unique,
        /// or an error message if it is not.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

            var existingEntity = dbContext.Languages.FirstOrDefault(l => l.Code == (string)value);

            if (existingEntity != null)
            {
                return new ValidationResult($"The code '{value}' is already in use.");
            }

            return ValidationResult.Success!;
        }
    }
}