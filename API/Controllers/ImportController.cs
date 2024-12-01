using API.Models;
using API.Services.DataImport;
using API.Services.Word;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly WordFacade _wordFacade;

    public ImportController(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    [HttpPost("words/{format}")]
    public IActionResult ImportWords(string format, [FromBody] string data)
    {
        try
        {
            List<WordPair> importedData;

            var importer = new DataImporter();

            switch (format.ToLower())
            {
                case "json":
                    importer.SetImportStrategy(new JsonImportStrategy(_wordFacade));
                    break;
                case "csv":
                    importer.SetImportStrategy(new CsvImportStrategy(_wordFacade));
                    break;
                case "xml":
                    importer.SetImportStrategy(new XmlImportStrategy(_wordFacade));
                    break;
                default:
                    return BadRequest($"Unsupported format '{format}'. Supported formats are: json, csv, xml.");
            }

            importedData = importer.ImportData(data);

            if (importedData.Count == 0)
            {
                return BadRequest("No valid word pairs were imported.");
            }

            foreach (var wordPair in importedData)
            {
                _wordFacade.AddWordPair(wordPair.Word, wordPair.Translation, wordPair.LanguageCode);
            }

            return Ok("Data imported successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error importing data: {ex.Message}");
        }
    }
}