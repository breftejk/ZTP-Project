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
        /// <summary>
        /// Gets the MIME type for CSV files.
        /// </summary>
        public string ContentType => "text/csv";

        /// <summary>
        /// Gets the file extension for CSV files.
        /// </summary>
        public string FileExtension => ".csv";

        /// <summary>
        /// Exports the given data to CSV format.
        /// </summary>
        /// <param name="data">The data to export.</param>
        /// <returns>A byte array containing the CSV data.</returns>
        public byte[] Export(IEnumerable<T> data)
        {
            // Retrieve properties of T, excluding those marked with CsvIgnoreAttribute
            var properties = typeof(T).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(CsvIgnoreAttribute)))
                .ToArray();

            var csvBuilder = new StringBuilder();

            // Append CSV header
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => Escape(p.Name))));

            // Append CSV rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null)?.ToString() ?? string.Empty;
                    return Escape(value);
                });
                csvBuilder.AppendLine(string.Join(",", values));
            }

            // Convert CSV string to byte array
            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }

        /// <summary>
        /// Escapes a CSV field by handling special characters.
        /// </summary>
        /// <param name="value">The value to escape.</param>
        /// <returns>The escaped CSV field.</returns>
        private string Escape(string value)
        {
            // Escape double quotes by replacing " with ""
            if (value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
            }

            // Enclose the field in quotes if it contains special characters
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                value = $"\"{value}\"";
            }

            return value;
        }
    }
}