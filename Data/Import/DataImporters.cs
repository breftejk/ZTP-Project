namespace ZTP_Project.Data.Import
{
    /// <summary>
    /// Factory for creating importers based on the specified format.
    /// </summary>
    public class DataImporters : IDataImporters
    {
        /// <inheritdoc />
        public IDataImporter<T> GetImporter<T>(string format) where T : new()
        {
            return format.ToLower() switch
            {
                "json" => new JsonDataImporter<T>(),
                "xml" => new XmlDataImporter<T>(),
                "csv" => new CsvDataImporter<T>(),
                _ => throw new NotSupportedException($"Format {format} is not supported.")
            };
        }
    }
}