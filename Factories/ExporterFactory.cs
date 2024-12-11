using ZTP_Project.Interfaces;
using ZTP_Project.Strategies.Exporters;

namespace ZTP_Project.Factories
{
    /// <summary>
    /// Factory for creating exporters based on the specified format.
    /// </summary>
    public class ExporterFactory : IExporterFactory
    {
        /// <inheritdoc />
        public IExporter<T> GetExporter<T>(string format)
        {
            return format.ToLower() switch
            {
                "json" => new JsonExporter<T>(),
                "xml" => new XmlExporter<T>(),
                "csv" => new CsvExporter<T>(),
                _ => throw new NotSupportedException($"Format {format} is not supported.")
            };
        }
    }
}