using System.Collections.Generic;

namespace ZTP_Project.Data.Import
{
    /// <summary>
    /// Defines a contract for importing data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data to import.</typeparam>
    public interface IDataImporter<T>
    {
        /// <summary>
        /// Imports data from a byte array.
        /// </summary>
        /// <param name="data">The byte array containing the data to import.</param>
        /// <returns>A collection of imported data of type <typeparamref name="T"/>.</returns>
        IEnumerable<T> Import(byte[] data);
    }
}