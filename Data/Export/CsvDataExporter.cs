using System.Text;
using ZTP_Project.Attributes;

namespace ZTP_Project.Data.Export
{
    /// <summary>
    /// Handles exporting data to CSV format.
    /// </summary>
    /// <typeparam name="T">The type of the data to export.</typeparam>
    public class CsvDataExporter<T> : IDataExporter<T>
    {
        public string ContentType => "text/csv";
        public string FileExtension => ".csv";

        /// <inheritdoc />
        public byte[] Export(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(CsvIgnoreAttribute)))
                .ToArray();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => Escape(p.Name))));

            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null)?.ToString() ?? string.Empty;
                    return Escape(value);
                });
                csvBuilder.AppendLine(string.Join(",", values));
            }

            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        private string Escape(string value)
        {
            if (value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
            }

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                value = $"\"{value}\"";
            }

            return value;
        }
    }
}