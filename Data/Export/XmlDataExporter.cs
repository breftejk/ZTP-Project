using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Handles exporting data to XML format.
    /// </summary>
    /// <typeparam name="T">The type of the data to export.</typeparam>
    public class XmlDataExporter<T> : IDataExporter<T>
    {
        /// <inheritdoc />
        public string ContentType => "application/xml";

        /// <inheritdoc />
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