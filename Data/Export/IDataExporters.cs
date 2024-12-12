using System;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Defines a contract for creating exporters based on the specified format.
    /// </summary>
    public interface IDataExporters
    {
        /// <summary>
        /// Retrieves an exporter for the specified format.
        /// </summary>
        /// <typeparam name="T">The type of data to export.</typeparam>
        /// <param name="format">The format for export (e.g., JSON, XML, CSV).</param>
        /// <returns>An instance of <see cref="IDataExporter{T}"/> for the specified format.</returns>
        /// <exception cref="NotSupportedException">Thrown when the specified format is not supported.</exception>
        IDataExporter<T> GetExporter<T>(string format);
    }
}