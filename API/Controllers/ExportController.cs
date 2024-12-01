using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Services.DataExport;

namespace API.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly WordFacade _wordFacade;

    public ExportController(WordFacade wordFacade)
    {
        _wordFacade = wordFacade;
    }

    [HttpGet("words/{language}/{format}")]
    public IActionResult ExportWords(string language, string format)
    {
        try
        {
            // Pobierz dane za pomocą WordFacade
            var data = _wordFacade.GetAllWords(language);

            if (data.Count == 0)
            {
                return NotFound($"No word pairs found for language '{language}'.");
            }

            // Użyj wzorca Strategii do wyboru formatu eksportu
            var exporter = new DataExporter();

            switch (format.ToLower())
            {
                case "json":
                    exporter.SetExportStrategy(new JsonExportStrategy());
                    break;
                case "csv":
                    exporter.SetExportStrategy(new CsvExportStrategy());
                    break;
                case "xml":
                    exporter.SetExportStrategy(new XmlExportStrategy());
                    break;
                default:
                    return BadRequest($"Unsupported format '{format}'. Supported formats are: json, csv, xml.");
            }

            // Wygeneruj wynik eksportu
            var result = exporter.ExportData(data);
            return Ok(result);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}