using System.Text;
using System.Text.Json;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Handles exporting data to JSON format.
    /// </summary>
    /// <typeparam name="T">The type of the data to export.</typeparam>
    public class JsonDataExporter<T> : IDataExporter<T>
    {
        public string ContentType => "application/json";
        public string FileExtension => ".json";

        /// <inheritdoc />
        public byte[] Export(IEnumerable<T> data)
        {
            var json = JsonSerializer.Serialize(data);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}