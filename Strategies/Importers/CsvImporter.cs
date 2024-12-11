using System.Text;
using ZTP_Project.Interfaces;

namespace ZTP_Project.Strategies.Importers
{
    /// <summary>
    /// Handles importing data from CSV format.
    /// </summary>
    /// <typeparam name="T">The type of the data to import.</typeparam>
    public class CsvImporter<T> : IImporter<T> where T : new()
    {
        /// <inheritdoc />
        public IEnumerable<T> Import(byte[] data)
        {
            var csv = Encoding.UTF8.GetString(data);
            var lines = csv.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var properties = typeof(T).GetProperties();

            var list = new List<T>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var obj = new T();

                for (int j = 0; j < properties.Length && j < values.Length; j++)
                {
                    var prop = properties[j];
                    var value = values[j].Trim('\"');

                    if (prop.PropertyType == typeof(int) && int.TryParse(value, out int intValue))
                    {
                        prop.SetValue(obj, intValue);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(obj, value);
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}