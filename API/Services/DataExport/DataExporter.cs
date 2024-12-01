using System.Collections.Generic;
using API.Models;

namespace API.Services.DataExport;

/// <summary>
/// Context for exporting data using a selected strategy.
/// </summary>
public class DataExporter
{
    private IDataExportStrategy _exportStrategy;

    /// <summary>
    /// Sets the export strategy dynamically.
    /// </summary>
    /// <param name="strategy">The export strategy to use.</param>
    public void SetExportStrategy(IDataExportStrategy strategy)
    {
        _exportStrategy = strategy;
    }

    /// <summary>
    /// Exports the given data using the selected strategy.
    /// </summary>
    /// <param name="data">The data to export.</param>
    /// <returns>The exported data as a string.</returns>
    public string ExportData(List<WordPair> data)
    {
        if (_exportStrategy == null)
        {
            throw new InvalidOperationException("Export strategy is not set.");
        }

        return _exportStrategy.Export(data);
    }
}