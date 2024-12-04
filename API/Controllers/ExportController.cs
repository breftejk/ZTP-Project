using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Strategies.DataExport;

namespace API.Controllers;

/// <summary>
/// API Controller for exporting word pairs in various formats.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly IWordService _wordService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportController"/> class.
    /// </summary>
    /// <param name="wordService">Service to handle word-related operations.</param>
    public ExportController(IWordService wordService)
    {
        _wordService = wordService ?? throw new ArgumentNullException(nameof(wordService));
    }

    /// <summary>
    /// Exports word pairs for a specific language or all languages in the requested format.
    /// </summary>
    /// <param name="languageCode">The language code for the word pairs to export. Use "all" for all languages.</param>
    /// <param name="format">
    /// The desired export format. Supported formats include:
    /// <list type="bullet">
    /// <item><term>json</term><description>JSON format</description></item>
    /// <item><term>csv</term><description>CSV format</description></item>
    /// <item><term>xml</term><description>XML format</description></item>
    /// </list>
    /// </param>
    /// <returns>The exported data in the specified format.</returns>
    /// <response code="200">Returns the exported data.</response>
    /// <response code="400">If the requested format is unsupported or invalid.</response>
    /// <response code="404">If no word pairs are found for the given language code.</response>
    [HttpGet("words/{languageCode}/{format}")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public IActionResult ExportWords(string languageCode, string format)
    {
        try
        {
            // Retrieve the word pairs based on the language code
            var wordPairs = languageCode.ToLower() == "all"
                ? _wordService.GetAllWordPairs(null)
                : _wordService.GetAllWordPairs(languageCode);

            if (!wordPairs.Any())
            {
                return NotFound($"No word pairs found for language '{languageCode}'.");
            }

            // Select the appropriate export strategy based on the format
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

            // Perform export and return the result
            var exportResult = exporter.ExportData(wordPairs);
            return Ok(exportResult);
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/// <summary>
/// Supported formats for data export.
/// </summary>
public enum ExportFormat
{
    Json,
    Csv,
    Xml
}