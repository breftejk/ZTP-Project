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

    [HttpPost("words")]
    [Consumes("text/csv", "application/json", "application/xml")]
    public IActionResult ImportWords([FromBody] string data)
    {
        try
        {
            string format = Request.ContentType?.ToLower() ?? string.Empty;
            List<WordPair> importedData;

            var importer = new DataImporter();

            switch (format)
            {
                case "application/json":
                    importer.SetImportStrategy(new JsonImportStrategy(_wordFacade));
                    break;

                case "text/csv":
                    importer.SetImportStrategy(new CsvImportStrategy(_wordFacade));
                    break;

                case "application/xml":
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

            return Ok("Data imported successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error importing data: {ex.Message}");
        }
    }
}