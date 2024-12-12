namespace ZTP_Project.Data.Import
{
    /// <summary>
    /// Factory interface for creating importers.
    /// </summary>
    public interface IDataImporters
    {
        /// <summary>
        /// Retrieves an importer for the specified format.
        /// </summary>
        /// <typeparam name="T">The type of data to import.</typeparam>
        /// <param name="format">The import format (e.g., JSON, XML, CSV).</param>
        /// <returns>An instance of an importer for the given format.</returns>
        IDataImporter<T> GetImporter<T>(string format) where T : new();
    }
}