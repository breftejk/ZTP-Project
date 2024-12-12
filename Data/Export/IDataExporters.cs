namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Factory interface for creating exporters.
    /// </summary>
    public interface IDataExporters
    {
        /// <summary>
        /// Retrieves an exporter for the specified format.
        /// </summary>
        /// <typeparam name="T">The type of data to export.</typeparam>
        /// <param name="format">The export format (e.g., JSON, XML, CSV).</param>
        /// <returns>An instance of an exporter for the given format.</returns>
        IDataExporter<T> GetExporter<T>(string format);
    }
}