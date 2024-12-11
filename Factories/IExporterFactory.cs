using ZTP_Project.Interfaces;

namespace ZTP_Project.Factories
{
    /// <summary>
    /// Factory interface for creating exporters.
    /// </summary>
    public interface IExporterFactory
    {
        /// <summary>
        /// Retrieves an exporter for the specified format.
        /// </summary>
        /// <typeparam name="T">The type of data to export.</typeparam>
        /// <param name="format">The export format (e.g., JSON, XML, CSV).</param>
        /// <returns>An instance of an exporter for the given format.</returns>
        IExporter<T> GetExporter<T>(string format);
    }
}