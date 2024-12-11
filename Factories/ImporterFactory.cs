using ZTP_Project.Interfaces;
using ZTP_Project.Strategies.Importers;

namespace ZTP_Project.Factories
{
    /// <summary>
    /// Factory for creating importers based on the specified format.
    /// </summary>
    public class ImporterFactory : IImporterFactory
    {
        /// <inheritdoc />
        public IImporter<T> GetImporter<T>(string format) where T : new()
        {
            return format.ToLower() switch
            {
                "json" => new JsonImporter<T>(),
                "xml" => new XmlImporter<T>(),
                "csv" => new CsvImporter<T>(),
                _ => throw new NotSupportedException($"Format {format} is not supported.")
            };
        }
    }
}