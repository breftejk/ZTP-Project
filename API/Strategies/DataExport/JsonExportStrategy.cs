using System.Text.Json;
using API.Models;

namespace API.Strategies.DataExport;

/// <summary>
/// Exports data in JSON format.
/// </summary>
public class JsonExportStrategy : IDataExportStrategy
{
    /// <inheritdoc />
    public string Export(List<WordPair> data)
    {
        return JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}