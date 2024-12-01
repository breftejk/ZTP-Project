using System.Text;
using API.Models;

namespace API.Services.DataExport;

/// <summary>
/// Exports data in CSV format.
/// </summary>
public class CsvExportStrategy : IDataExportStrategy
{
    /// <inheritdoc />
    public string Export(List<WordPair> data)
    {
        var builder = new StringBuilder();

        // Add header row
        builder.AppendLine("Word,Translation,Language");

        // Add data rows
        foreach (var pair in data)
        {
            builder.AppendLine($"{pair.Word},{pair.Translation},{pair.Language}");
        }

        return builder.ToString();
    }
}