using System.Text;
using API.Models;

namespace API.Strategies.DataExport;

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
        builder.AppendLine("Id,LanguageCode,Word,Translation");

        // Add data rows
        foreach (var pair in data)
        {
            builder.AppendLine($"{pair.Id},{pair.LanguageCode},{pair.Word},{pair.Translation}");
        }

        return builder.ToString();
    }
}