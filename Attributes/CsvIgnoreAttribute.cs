namespace ZTP_Project.Attributes
{
    /// <summary>
    /// Attribute used to mark properties that should be ignored during CSV serialization or deserialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CsvIgnoreAttribute : Attribute
    {
    }
}