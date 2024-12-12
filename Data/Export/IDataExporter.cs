using System.Collections.Generic;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Defines a contract for exporting data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data to export.</typeparam>
    public interface IDataExporter<T>
    {
        /// <summary>
        /// Exports the given data into a specific format.
        /// </summary>
        /// <param name="data">The data to export.</param>
        /// <returns>A byte array containing the exported data.</returns>
        byte[] Export(IEnumerable<T> data);

        /// <summary>
        /// Gets the MIME content type of the export format.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the file extension of the export format.
        /// </summary>
        string FileExtension { get; }
    }
}