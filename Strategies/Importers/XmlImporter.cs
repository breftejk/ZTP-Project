using System.Xml.Serialization;
using ZTP_Project.Interfaces;

namespace ZTP_Project.Strategies.Importers
{
    /// <summary>
    /// Handles importing data from XML format.
    /// </summary>
    /// <typeparam name="T">The type of the data to import.</typeparam>
    public class XmlImporter<T> : IImporter<T>
    {
        /// <inheritdoc />
        public IEnumerable<T> Import(byte[] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Data is null or empty.", nameof(data));

            var rootName = "ListOf" + typeof(T).Name;
            var serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootName));

            using (var memoryStream = new MemoryStream(data))
            {
                try
                {
                    return serializer.Deserialize(memoryStream) as List<T> ?? new List<T>();
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException("Deserialization failed.", ex);
                }
            }
        }
    }
}