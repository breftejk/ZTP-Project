using API.Models;
using API.Strategies.DataImport;

namespace API.Services;

public class DataImporter
{
    private IDataImportStrategy _importStrategy;

    public void SetImportStrategy(IDataImportStrategy strategy)
    {
        _importStrategy = strategy;
    }

    public List<WordPair> ImportData(string data)
    {
        if (_importStrategy == null)
        {
            throw new InvalidOperationException("Import strategy is not set.");
        }

        return _importStrategy.Import(data);
    }
}