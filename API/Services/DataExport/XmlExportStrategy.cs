using System.Xml.Serialization;
using API.Models;

namespace API.Services.DataExport;

/// <summary>
/// Exports data in XML format.
/// </summary>
public class XmlExportStrategy : IDataExportStrategy
{
    /// <inheritdoc />
    public string Export(List<WordPair> data)
    {
        var serializer = new XmlSerializer(typeof(List<WordPair>));
        using var stringWriter = new StringWriter();

        serializer.Serialize(stringWriter, data);
        return stringWriter.ToString();
    }
}