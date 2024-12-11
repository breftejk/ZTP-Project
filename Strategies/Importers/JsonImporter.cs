using System.Text;
using System.Text.Json;
using ZTP_Project.Interfaces;

namespace ZTP_Project.Strategies.Importers
{
    /// <summary>
    /// Handles importing data from JSON format.
    /// </summary>
    /// <typeparam name="T">The type of the data to import.</typeparam>
    public class JsonImporter<T> : IImporter<T>
    {
        /// <inheritdoc />
        public IEnumerable<T> Import(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<IEnumerable<T>>(json) ?? new List<T>();
        }
    }
}