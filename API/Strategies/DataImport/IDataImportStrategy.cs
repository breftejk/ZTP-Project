using API.Models;

namespace API.Strategies.DataImport;

public interface IDataImportStrategy
{
    List<WordPair> Import(string data);
}