using System;
using ZTP_Project.Attributes;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Factory for creating exporters based on the specified format.
    /// </summary>
    public class DataExporters : IDataExporters
    {
        /// <inheritdoc />
        public IDataExporter<T> GetExporter<T>(string format)
        {
            return format.ToLower() switch
            {
                "json" => new JsonDataExporter<T>(),
                "xml" => new XmlDataExporter<T>(),
                "csv" => new CsvDataExporter<T>(),
                _ => throw new NotSupportedException($"Format {format} is not supported.")
            };
        }
    }
}