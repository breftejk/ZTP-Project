using API.Models;

namespace API.Services.DataImport;

public interface IDataImportStrategy
{
    List<WordPair> Import(string data);
}