using System.Xml.Serialization;
using ZTP_Project.Interfaces;

namespace ZTP_Project.Strategies.Exporters
{
    /// <summary>
    /// Handles exporting data to XML format.
    /// </summary>
    /// <typeparam name="T">The type of the data to export.</typeparam>
    public class XmlExporter<T> : IExporter<T>
    {
        public string ContentType => "application/xml";
        public string FileExtension => ".xml";

        /// <inheritdoc />
        public byte[] Export(IEnumerable<T> data)
        {
            var dataList = data as List<T> ?? new List<T>(data);
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, dataList);
                return memoryStream.ToArray();
            }
        }
    }
}