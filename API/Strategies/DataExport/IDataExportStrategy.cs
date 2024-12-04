using API.Models;

namespace API.Strategies.DataExport;

/// <summary>
/// Interface for exporting data in different formats.
/// </summary>
public interface IDataExportStrategy
{
    /// <summary>
    /// Exports a list of word pairs to the desired format.
    /// </summary>
    /// <param name="data">The data to export.</param>
    /// <returns>The exported data as a string.</returns>
    string Export(List<WordPair> data);
}