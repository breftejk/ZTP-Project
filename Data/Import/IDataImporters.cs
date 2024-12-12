using System;

namespace ZTP_Project.Data.Import
{
    /// <summary>
    /// Defines a contract for creating importers based on the specified format.
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